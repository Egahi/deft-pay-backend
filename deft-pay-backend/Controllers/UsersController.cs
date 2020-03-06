using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using deft_pay_backend.Models;
using deft_pay_backend.ModelsDTO.Responses;
using deft_pay_backend.Repositories.Interfaces;
using deft_pay_backend.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace deft_pay_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IUserRepository UserRepository { get; }
        public ITransactionTokenRepository TransactionTokenRepository { get; }
        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public UsersController(IUserRepository userRepository,
                               ITransactionTokenRepository transactionTokenRepository,
                               ILogger<AccountsController> logger,
                               IMapper mapper)
        {
            UserRepository = userRepository;
            TransactionTokenRepository = transactionTokenRepository;
            Logger = logger;
            Mapper = mapper;
        }

        /// <summary>Get Users endpoint</summary>
        /// <remarks>
        /// Retrieves a paginated list of all registered users
        /// Requires Authorization
        /// </remarks>
        /// <response code="200">Success</response>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(DataResponseArrayDTO<UserProfileSummaryDTO>), StatusCodes.Status200OK)]
        public IActionResult Get(int page = 0, int size = 20)
        {
            Logger.LogInformation("UsersController Get method called");

            if (page < 0)
                page = 0;

            if (size < 0)
                size = 20;
            int skip = (size * page);

            // svar currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //IEnumerable<ApplicationUser> users;
            //if (User.IsInRole(UserRoleConstants.ADMIN))
            //{
            //    users = UserRepository.Get();
            //}
            //else
            //{
            //    users = UserRepository.Get(x => x.Id == new Guid(currentUserId));
            //}
            
            var users = UserRepository.Get();

            var totalUsers = users.Count();
            var pageUsers = users.Skip(skip).Take(size).ToList();

            var userProfiles = Mapper.Map<List<UserProfileSummaryDTO>>(pageUsers);
            return Ok(new DataResponseArrayDTO<UserProfileSummaryDTO>(userProfiles, totalUsers, page, size));
        }

        /// <summary>Get Users endpoint</summary>
        /// <remarks>
        /// Retrieves a paginated list of all registered users
        /// Requires Authorization
        /// </remarks>
        /// <response code="200">Success</response>
        /// <param name="bvn"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpGet("{bvn}/token")]
        [ProducesResponseType(typeof(DataResponseArrayDTO<TransactionTokenResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateErrorResponseDTO), StatusCodes.Status400BadRequest)]
        public IActionResult GetToken([FromRoute]string bvn, [FromQuery]string amount = "0.00")
        {
            Logger.LogInformation("UsersController GetToken method called");

            ApplicationUser user = null;
            try
            {
                // TODO verify user from API
                user = UserRepository.Get(x => x.BVN == bvn)
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

            var transactionToken = new TransactionToken();

            try
            {
                transactionToken.Amount = double.Parse(amount);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while logging user in");
                return BadRequest(new ErrorResponseDTO(HttpStatusCode.BadRequest,
                    new string[] { "Invalid amount entered. Please enter a valid amount for the transaction" }));
            }

            transactionToken.UserId = user.Id;
            while (TransactionTokenRepository.Get(x => x.OTP == transactionToken.OTP).Count() != 0)
            {
                transactionToken.OTP = Helper.GetRandomToken(30);
            }

            try
            {
                TransactionTokenRepository.Insert(transactionToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while logging user in");
                return BadRequest(new ErrorResponseDTO(HttpStatusCode.BadRequest,
                    new string[] { "Could not complete request. Please retry later, or contact the support team" }));
            }

            var transactionTokenDTO = Mapper.Map<TransactionTokenResponseDTO>(transactionToken);
            transactionTokenDTO.User = Mapper.Map<UserProfileSummaryDTO>(user);

            return Ok(new DataResponseDTO<TransactionTokenResponseDTO>(transactionTokenDTO, HttpStatusCode.OK));
        }

    }
}