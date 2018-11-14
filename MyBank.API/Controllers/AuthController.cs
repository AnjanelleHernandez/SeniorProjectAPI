using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyBank.API.Data;
using MyBank.API.Dtos;
using MyBank.API.Models;

namespace MyBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
      {
          _repo = repo;
          _config = config;
        }  

      [HttpPost("register")]
      public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
      {
        if(await _repo.UserExists(userForRegisterDto.EmailAddress))
        {
            return BadRequest("Email Associated with another account");
        }
        var userToCreate = new User
            {
                firstName = userForRegisterDto.FirstName,
                lastName = userForRegisterDto.LastName,
                emailAddress = userForRegisterDto.EmailAddress
            };
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);
            return StatusCode(201);
      }

      [HttpPost("login")]
      public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
      {
          var userFromRepo = await _repo.Login(userForLoginDto.EmailAddress, userForLoginDto.Password);

          if(userFromRepo == null)
              return Unauthorized();

          var claims = new[]
          {
              new Claim(ClaimTypes.NameIdentifier, userFromRepo.ID.ToString()),
              new Claim(ClaimTypes.Name, userFromRepo.firstName)
          };

          var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("App Settings:Token").Value));

          var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
          
          var tokenDescriptor = new SecurityTokenDescriptor
          {
              Subject = new ClaimsIdentity(claims),
              Expires = DateTime.Now.AddDays(1),
              SigningCredentials = creds
          };

          var tokenHandler = new JwtSecurityTokenHandler();
          var token = tokenHandler.CreateToken(tokenDescriptor);
          return Ok( new {
              token = tokenHandler.WriteToken(token)
          });
      }
    }
}