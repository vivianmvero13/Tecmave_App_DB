using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AccountController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        
        public record LoginDTO(string Email, string Password);
        public record LoginResponseDTO(int Id, string Nombre, string Apellidos, string UserName, string Email, List<string> Roles);
        public record RegisterDTO(string Nombre, string Apellidos, string UserName, string Email, string Password);
        public record RegisterResponseDTO(int Id, string Nombre, string Apellidos, string UserName, string Email, List<string> Roles);

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("Usuario y/o contraseña son incorrectos");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return Unauthorized("Usuario y/o contraseña son incorrectos");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new LoginResponseDTO(
                user.Id,
                user.Nombre ?? "",
                user.Apellido ?? "",
                user.UserName ?? "",
                user.Email ?? "",
                roles.ToList()
            ));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing == null) return BadRequest("Ya existe un usuario que tiene ese correo");

            var user = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellidos,
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Cliente");

            var roles = await _userManager.GetRolesAsync(user);

            return Created("", new RegisterResponseDTO(
                user.Id,
                user.Nombre ?? "",
                user.Apellido ?? "",
                user.UserName ?? "",
                user.Email ?? "",
                roles.ToList()
            ));
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new LoginResponseDTO(
                user.Id,
                user.Nombre ?? "",
                user.Apellido ?? "",
                user.UserName ?? "",
                user.Email ?? "",
                roles.ToList()
            ));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}