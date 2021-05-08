using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UsersApi.BusinessObjects;
using UsersApi.Converters;
using UsersApi.Repository;
using UsersApi.Validators;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserValidator _validator;
        private readonly IUsersRepository _usersRepository;
        private readonly IUserConverter _userConverter;

        public UsersController(
            ILogger<UsersController> logger,
            IUserValidator validator,
            IUsersRepository usersRepository,
            IUserConverter userConverter
            )
        {
            _logger = logger;
            _validator = validator;
            _usersRepository = usersRepository;
            _userConverter = userConverter;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([NotNull] User user)
        {
            var validateResult = _validator.Validate(user);
            if (!validateResult.IsSuccess)
            {
                return BadRequest(validateResult.ErrorMessage);
            }
            
            var result = await _usersRepository.CreateAsync(_userConverter.ToDto(user));
            return result.IsSuccess 
                ? (ActionResult) Ok() 
                : BadRequest(result.ErrorMessage);
        }
    }
}