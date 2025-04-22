using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeycloakWithDotNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateToken(LoginRequest request)
        {
            var tokenEndpoint = $"{_configuration["Keycloak:Authority"]}/protocol/openid-connect/token";

            var requestData = new Dictionary<string, string>
            {
                ["client_id"] = _configuration["Keycloak:ClientId"],
                ["client_secret"] = _configuration["Keycloak:ClientSecret"],
                ["grant_type"] = "password",
                ["username"] = request.Username,
                ["password"] = request.Password
            };

            var content = new FormUrlEncodedContent(requestData);
            var response = await _httpClient.PostAsync(tokenEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                return Ok(jsonContent);
            }

            return BadRequest($"Failed to get token: {await response.Content.ReadAsStringAsync()}");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
