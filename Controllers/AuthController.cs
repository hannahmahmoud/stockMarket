using backend.DTOs;
using backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(
            AuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterDto registerDto)
        {
            try
            {
                var token =
                    await authService.Register(
                        registerDto
                    );

                return Ok(new
                {
                    message =
                        "Registration successful",

                    token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginDto loginDto)
        {
            var token =
                await authService.Login(
                    loginDto
                );

            if (token == null)
            {
                return Unauthorized(new
                {
                    message =
                        "Invalid email or password"
                });
            }

            return Ok(new
            {
                message =
                    "Login successful",

                token = token
            });
        }
    }
}