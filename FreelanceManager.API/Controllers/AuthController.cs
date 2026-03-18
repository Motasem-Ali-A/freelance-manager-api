using FreelanceManager.Core.DTOs.Auth;
using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        public AuthController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService= tokenService;
        }

        /// <summary>
        /// Register (new user)
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new AppUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FullName = dto.FullName,
                BusinessName = dto.BusinessName
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok("User registered successfully");
        }

        /// <summary>
        /// Login (existing user)
        /// </summary>
        /// <returns>Uesr details and JWT token</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto),200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return BadRequest("User not registered");
            }
            else if (!(await _userManager.CheckPasswordAsync(user, dto.Password)))
            {
                return Unauthorized();
            }

            return Ok(new AuthResponseDto
            {
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Token = _tokenService.CreateToken(user)
            });
        }

    }
}
