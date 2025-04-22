using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KeycloakWithDotNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new { message = "This is a public endpoint - no authentication required" });
        }

        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            return Ok(new
            {
                message = "This endpoint is protected - authentication succeeded!",
                userId,
                userName,
                email,
                roles = userRoles,
                allClaims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "admin")]
        public IActionResult AdminOnly()
        {
            return Ok(new { message = "This endpoint is for administrators only" });
        }

        [HttpGet("config")]
        public IActionResult GetKeycloakConfig()
        {
            var keycloakConfig = new
            {
                Authority = _configuration["Keycloak:Authority"],
                Audience = _configuration["Keycloak:Audience"],
                Realm = _configuration["Keycloak:Realm"],
                IsEnabled = _configuration.GetValue<bool>("Keycloak:Enabled")
            };

            return Ok(keycloakConfig);
        }
    }
}
