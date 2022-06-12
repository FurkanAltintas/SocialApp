using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerApp.DTOs;
using ServerApp.Models;
using ServerApp.Security;

namespace ServerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Name = userForRegisterDTO.Name,
                Email = userForRegisterDTO.Email,
                UserName = userForRegisterDTO.UserName,
                Gender = userForRegisterDTO.Gender,
                DateOfBirth = userForRegisterDTO.DateOfBirth,
                Country = userForRegisterDTO.Country,
                City = userForRegisterDTO.City,
                Created = DateTime.Now,
                LastActive = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, userForRegisterDTO.Password);

            if (result.Succeeded)
            {
                return StatusCode(201);
            }
            
            return BadRequest(result.Errors);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDTO.UserName);

            if (user == null)
                return BadRequest(new { message = "UserName is incorrect." });
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDTO.Password, false); // 3. parametre kullanıcının hatalı giriş yapması durumunda hesabının kilitlenip kilitlenmeyeceğini yazıyoruz.

            if (result.Succeeded)
            {
                return Ok(new {
                    token = new GenerateToken(_configuration).CreateToken(user)
                });
            }

            return Unauthorized();
        }
    }
}