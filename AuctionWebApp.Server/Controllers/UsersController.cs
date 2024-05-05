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

            var tokens = await userService.Login(loginModel);
            if (tokens is null)
            {
                return Unauthorized();
            }

            return Ok(tokens);
        }

        [HttpPost("registrate")]
        public IActionResult RegistrateAsync([FromBody] LoginInfo registrationInfo)
        {
            return BadRequest();
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
        public async Task<IActionResult> RevokAsynce()
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
    }
}
