using ChneseSaleApi.buisness;
using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sale.buisness;
using System;
using System.Threading.Tasks;

namespace sale.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Customer customer)
        {
            _logger.LogInformation("Starting registration process");

            try
            {
                var result = await _service.register(customer);
                _logger.LogInformation("Registration completed successfully for user {UserName}", customer.LastName);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for user {UserName}", customer.LastName);
                return StatusCode(500, "An error occurred while processing the registration.");
            }
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(string firstName, string lastName, string password)
        {
            _logger.LogInformation("Starting login process for user {FirstName} {LastName}", firstName, lastName);

            try
            {
                var result = await _service.login(firstName, lastName, password);
                _logger.LogInformation("Login completed successfully for user {FirstName} {LastName}", firstName, lastName);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user {FirstName} {LastName}", firstName, lastName);
                return StatusCode(500, "An error occurred while processing the login.");
            }
        }
    }
}
