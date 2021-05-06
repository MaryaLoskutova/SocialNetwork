using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UsersApi.BusinessObjects;
using UsersApi.Validators;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserValidator _validator;

        public UsersController(
            ILogger<UsersController> logger,
            IUserValidator validator
            )
        {
            _logger = logger;
            _validator = validator;
        }

        [HttpPost("register")]
        public ActionResult Register([NotNull] User user)
        {
            var validateResult = _validator.Validate(user);
            if (!validateResult.IsSuccess)
            {
                return BadRequest(validateResult.ErrorMessage);
            }

            return Ok();
        }
    }
}