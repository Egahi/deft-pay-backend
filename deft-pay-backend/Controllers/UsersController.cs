using System;
using System.Collections.Generic;
using System.Linq;
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
        private ILogger Logger { get; }

        private IMapper Mapper { get; }

        public UsersController(IUserRepository userRepository,
                               ILogger<AccountsController> logger,
                               IMapper mapper)
        {
            UserRepository = userRepository;
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
        [Authorize]
        [ProducesResponseType(typeof(DataResponseArrayDTO<ApplicationUser>), StatusCodes.Status200OK)]
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

            //var userProfiles = Mapper.Map<List<ApplicationUser>>(pageUsers);
            return Ok(new DataResponseArrayDTO<ApplicationUser>(pageUsers, totalUsers, page, size));
        }

    }
}