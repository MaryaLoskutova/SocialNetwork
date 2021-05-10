using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsersApi.BusinessObjects;
using UsersApi.Converters;
using UsersApi.Services;
using UsersApi.Validators;

namespace UsersApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserValidator _validator;
        private readonly IUsersService _usersService;
        private readonly IUserConverter _userConverter;

        public UsersController(
            IUserValidator validator,
            IUserConverter userConverter,
            IUsersService usersService
            )
        {
            _validator = validator;
            _userConverter = userConverter;
            _usersService = usersService;
        }

        
        /// <include file='Documentation/UsersController.xml' path='Controller/Action[@name="Register"]/*'/>
        [ProducesResponseType(typeof(Guid), 200)]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([NotNull] UserRegistrationInfo userRegistrationInfo)
        {
            var validateResult = _validator.Validate(userRegistrationInfo);
            if (!validateResult.IsSuccess)
            {
                return BadRequest(validateResult.ErrorMessage);
            }
            
            var result = await _usersService.RegisterAsync(_userConverter.ToDto(userRegistrationInfo));
            return result.IsSuccess 
                ? (ActionResult) Ok(result.Value) 
                : BadRequest(result.ErrorMessage);
        }
        
        
        /// <include file='Documentation/UsersController.xml' path='Controller/Action[@name="Subscribe"]/*'/>
        [ProducesResponseType(typeof(Guid), 200)]
        [HttpPost("{subscriberId}/subscribe")]
        public async Task<ActionResult> SubscribeAsync([NotNull] Guid subscriberId, [NotNull] Guid userId)
        {
            var result = await _usersService.SubscribeAsync(subscriberId, userId);
            return result.IsSuccess 
                ? (ActionResult) Ok(result.Value) 
                : BadRequest(result.ErrorMessage);
        }
        
        
        /// <include file='Documentation/UsersController.xml' path='Controller/Action[@name="Top"]/*'/>
        [ProducesResponseType(typeof(UserDto[]), 200)]
        [HttpGet("top")]
        public async Task<ActionResult> TopAsync([NotNull] int count)
        {
            var result = await _usersService.SelectTopPopularAsync(count);
            return Ok(result);
        }
    }
}