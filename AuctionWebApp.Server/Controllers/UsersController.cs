using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController(IUserService userService) : Controller
    {
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginInfo loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userInfo = await userService.Login(loginModel);
            if (userInfo is null)
            {
                return Unauthorized();
            }

            return Ok(userInfo);
        }

        [HttpPost("registrate")]
        public async Task<IActionResult> RegistrateAsync([FromBody] RegistrationInfo registrationInfo)
        {
            if (!ModelState.IsValid || !await userService.Registration(registrationInfo))
            {
                return BadRequest();
            }

            var loginModel = new LoginInfo()
            {
                Email = registrationInfo.Email,
                Password = registrationInfo.PasswordHash
            };
            var userShort = await userService.Login(loginModel);
            if (userShort is null)
            {
                return Unauthorized();
            }

            return Ok(userShort);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(TokenApiModel tokenApiModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var tokens = await userService.RefreshToken(tokenApiModel);
            if (tokens is null)
            {
                return Unauthorized();
            }

            return Ok(tokens);
        }

        [HttpPost("revoke"), Authorize]
        public async Task<IActionResult> RevokeAsynce()
        {
            if (!ModelState.IsValid || User.Identity is null)
            {
                return BadRequest();
            }

            var email = User.Identity.Name;
            if (email is null || !await userService.RevokeToken(email))
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet("roles/{roleId}")]
        public async Task<IActionResult> GetRoleName(byte roleId)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.GetRoleName(roleId);
                return (result != null) ? Json(result) : NoContent();
            }

            return BadRequest();
        }
    }
}
