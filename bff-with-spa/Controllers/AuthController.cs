using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace bffwithspa.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login([FromServices] IConfiguration configuration, [FromQuery] string redirectUri)
        {
            redirectUri = redirectUri.Trim('/');
            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
            // Validate the provided redirectUri
            if (string.IsNullOrEmpty(redirectUri) || !allowedOrigins!.Contains(redirectUri))
            {
                return BadRequest("Invalid redirect URI");
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri // Use the validated redirect URI
            };
            return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromServices] IConfiguration configuration, [FromQuery] string redirectUri)
        {
            redirectUri = redirectUri.Trim('/');
            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
            // Validate the provided redirectUri
            if (string.IsNullOrEmpty(redirectUri) || !allowedOrigins!.Contains(redirectUri))
            {
                return BadRequest("Invalid redirect URI");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = redirectUri // Redirect to SPA after logout
            });
            return Ok();
        }

        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            if (!User.Identity?.IsAuthenticated == true)
            {
                return Unauthorized();
            }

            var userInfo = new
            {
                name = User.FindFirst("name")?.Value ?? User.Identity?.Name,
                email = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value,
                object_id = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ?? User.FindFirst("id")?.Value,
                session_id = User.FindFirst("sid")?.Value,
                // Add other claims you want to expose
            };

            return Ok(userInfo);
        }

        [HttpGet("is-authenticated")]
        [AllowAnonymous]
        public IActionResult IsAuthenticated()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Ok(new
                {
                    isAuthenticated = true,
                    name = User.Identity.Name ?? User.FindFirst("name")?.Value,
                    email = User.FindFirst("email")?.Value
                });
            }
            return Ok(new { isAuthenticated = false });
        }
    }
}
