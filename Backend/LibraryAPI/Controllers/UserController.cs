using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPut("Registration")]
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Creates the user and returns a boolean",
            OperationId = "Registration"
            )]
        [SwaggerResponse(200, "Registration complete!", typeof(User))]
        public async Task <ActionResult<User>> Register([FromBody] UserForRegister newUser)
        {
            var oid = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            await _userService.VerifyRegisterAsync(newUser, oid);
            return Ok(true);
        }

        [HttpGet("CheckExistingUser")]
        [SwaggerOperation(
            Summary = "Check if a user exists",
            Description = "Checks if the user exists.",
            OperationId = "CheckExistingUser"
            )]
        [SwaggerResponse(200, "User found!", typeof(User))]
        public ActionResult<bool> CheckExistingUser()
        {
            var oid = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            var user = _userService.CheckUserInDb(oid);

            if(user.Result == true)
            {
                return Ok(true);
            } else
            {
                return NotFound("User not found");
            }
        }

        [HttpGet("GetUserInfo")]
        [SwaggerOperation(
            Summary = "Get all information about the user",
            Description = "Get username, phone, adress, city and postalcode.",
            OperationId = "GetUserInfo"
            )]
        [SwaggerResponse(200, "User information found!", typeof(User))]
        public async Task <ActionResult<User>> GetUserInfo()
        {
            var oid = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            var user = await _userService.GetUserInformation(oid);

            if(user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPut("UpdateUserInfo")]
        [SwaggerOperation(
            Summary = "Save new user data",
            Description = "Save new username, phone, adress, city and postalcode.",
            OperationId = "UpdateUserInfo"
            )]
        [SwaggerResponse(200, "User information updated!", typeof(User))]
        public async Task <ActionResult<User>> UpdateUserInfo([FromBody] UserForDataUpdate userData)
        {
            var oid = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            var user = await _userService.UpdateUserInfo(userData, oid);

            if (!user)
            {
                return NotFound("User not found");
            }

            return Ok("User information updated!");
        }

        [HttpGet("GetUserRegistrationState")]
        [SwaggerOperation(
            Summary = "Check if the user allready has completed the registration.",
            Description = "Get true or false",
            OperationId = "GetUserRegistrationState"
            )]
        [SwaggerResponse(200, "Registration state found!", typeof(User))]
        public async Task<ActionResult<bool>> GetUserRegistrationState()
        {
            var oid = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            var registrationComplete = await _userService.CheckRegistrationState(oid);

            if (oid == null)
            {
                return BadRequest("User identifier is missing.");
            }

            if (!registrationComplete)
            {
                return NotFound("User not found or registration incomplete.");
            }

            return Ok(true);
        }

        [HttpPut("SetRegistrationStateToTrue")]
        [SwaggerOperation(
            Summary = "Sets the users registration state to true.",
            Description = "Get true or false",
            OperationId = "SetRegistrationStateToTrue"
            )]
        [SwaggerResponse(200, "Registration state set!", typeof(User))]
        public async Task<ActionResult<bool>> SetRegistrationStateToTrue()
        {
            var oid = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            var registrationSet = await _userService.SetRegistrationStateToTrue(oid);

            if (oid == null)
            {
                return BadRequest("User identifier is missing.");
            }

            if (!registrationSet)
            {
                return NotFound("User not found or registration allready complete.");
            }

            return Ok(registrationSet);
        }
    }
}
