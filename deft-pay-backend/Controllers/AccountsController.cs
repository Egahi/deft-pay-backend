using AutoMapper;
using deft_pay_backend.Models;
using deft_pay_backend.ModelsDTO.Requests;
using deft_pay_backend.ModelsDTO.Responses;
using deft_pay_backend.Repositories.Interfaces;
using deft_pay_backend.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace deft_pay_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IUserRepository UserRepository { get; }
        public IConfiguration Configuration { get; }
        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public AccountsController(UserManager<ApplicationUser> userManager,
                                  IRefreshTokenRepository refreshTokenRepository,
                                  IConfiguration configuration,
                                  IUserRepository userRepository,
                                  ILogger<AccountsController> logger,
                                  IMapper mapper)
        {
            UserManager = userManager;
            RefreshTokenRepository = refreshTokenRepository;
            UserRepository = userRepository;
            Configuration = configuration;
            Logger = logger;
            Mapper = mapper;
        }

        /// <summary>User registration endpoint</summary>
        /// <remarks>
        /// Requires either a unique email or phone number
        /// Gender must be either 'Male' or 'Female'.
        /// Birthdate format dd/MM/yyyy
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(DataResponseDTO<LoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostApplicationUser([FromBody]UserRegisterDTO model)
        {
            Logger.LogError("PostApplicationUser method called");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ModelStateErrorResponseDTO(HttpStatusCode.BadRequest, ModelState));
            }
            
            var applicationUser = await UserManager.FindByNameAsync(model.BVN);

            if (applicationUser != null)
            {
                return BadRequest(new ErrorResponseDTO(HttpStatusCode.BadRequest,
                        new string[] { "You already have an account on this platfrom proceed to login." }));
            }

            applicationUser = Mapper.Map<ApplicationUser>(model);

            try
            {
                await UserManager.CreateAsync(applicationUser);

                await UserManager.AddToRoleAsync(applicationUser, UserRoleConstants.USER);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while registering");
                return BadRequest(new ErrorResponseDTO(HttpStatusCode.BadRequest,
                    new string[] { "Could not complete your registration. Please retry later, or contact the support team" }));
            }

            // TODO
            // var response = Helper.MakeAPICall("face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=[age,gender]");

            return Ok(GetJWTToken(applicationUser));
        }

        /// <summary>Login endpoint</summary>
        /// <remarks>Accepts either email or phone number as username</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(DataResponseDTO<LoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ModelStateErrorResponseDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO model)
        {
            Logger.LogError("Login method called");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ModelStateErrorResponseDTO(HttpStatusCode.BadRequest, ModelState));
            }

            ApplicationUser user = null;
            try
            {
                // TODO verify user from API
                user = UserRepository.Get(x => x.BVN == model.BVN)
                                     .FirstOrDefault();

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while logging user in");
                return BadRequest(new ErrorResponseDTO(HttpStatusCode.BadRequest,
                    new string[] { "Could not complete request. Please retry later, or contact the support team" }));
            }

            if (user == null || user.ShouldDelete)
            {
                return BadRequest(new ErrorResponseDTO(HttpStatusCode.BadRequest,
                    new List<string> { "You do not have an account with us kindly proceed to signup." }));
            }

            return Ok(GetJWTToken(user));
        }

        /// <summary>Refresh token endpoint</summary>
        /// <remarks>Requires a valid refreshtoken and userid</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DataResponseDTO<LoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateErrorResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenDTO model)
        {
            Logger.LogError("RefreshToken method called");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ModelStateErrorResponseDTO(HttpStatusCode.BadRequest, ModelState));
            }

            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null || user.ShouldDelete)
            {
                return NotFound(new ErrorResponseDTO(HttpStatusCode.NotFound,
                    new string[] { "The user was not found" }));
            }

            var token = RefreshTokenRepository
                .Get(x => x.RefreshTokenId == model.RefreshToken && x.ExpiryTime > DateTime.UtcNow)
                .FirstOrDefault();

            if (token != null)
            {
                RefreshTokenRepository.Delete(model.RefreshToken);
                return Ok(GetJWTToken(user));
            }

            return BadRequest(new ErrorResponseDTO(HttpStatusCode.BadRequest,
                new string[] { "The token is invalid" }));
        }

        /// <summary>Logout endpoint</summary>
        /// <remarks>Requires Authorization</remarks>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(DataResponseDTO<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateErrorResponseDTO), StatusCodes.Status400BadRequest)]
        public IActionResult Logout([FromBody]LogoutRequestDTO model)
        {
            Logger.LogError("Logout method called");

            if (!ModelState.IsValid)
            {
                return BadRequest(new ModelStateErrorResponseDTO(
                    HttpStatusCode.BadRequest, ModelState));
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var token = RefreshTokenRepository
                .Get(x => x.RefreshTokenId == model.RefreshToken && x.User.Id == new Guid(currentUserId))
                .FirstOrDefault();

            if (token != null)
            {
                RefreshTokenRepository.Delete(model.RefreshToken);
            }

            return Ok(new DataResponseDTO<string>("Logout successful"));
        }

        private DataResponseDTO<LoginResponseDTO> GetJWTToken(ApplicationUser user)
        {
            var currentTime = DateTime.UtcNow;
            var userRoles = UserManager.GetRolesAsync(user).Result;
            Logger.LogError($"-------- There are {userRoles.Count} roles in GetJWTToken for user {user.Email}");
            IdentityOptions identityOptions = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(identityOptions.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(identityOptions.ClaimsIdentity.UserNameClaimType, user.UserName)
            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(identityOptions.ClaimsIdentity.RoleClaimType, role));
                Logger.LogError($"---------- ADDING ROLE {role} to the roles for user {user.Email}");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = currentTime.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT_Secret"].ToString())),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                ExpiryTime = currentTime.AddDays(30),
                GeneratedTime = currentTime
            };
            do
            {
                refreshToken.RefreshTokenId = Helper.GetRandomToken(96);
            }
            while (RefreshTokenRepository.GetByID(refreshToken.RefreshTokenId) != null);
            RefreshTokenRepository.Insert(refreshToken);

            return new DataResponseDTO<LoginResponseDTO>(new LoginResponseDTO
            {
                Id = user.Id,
                Token = token,
                RefreshToken = refreshToken.RefreshTokenId,
                ExpiryTime = tokenDescriptor.Expires.ToString(),
                Roles = userRoles,
                Fullname = string.Join(" ", new List<string> { user.FirstName, user?.MiddleName, user.LastName }),
                BVN = user.BVN
            });
        }
    }
}