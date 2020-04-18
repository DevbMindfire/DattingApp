using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DattingApp.API.Data.Repository;
using DattingApp.API.DTO;
using DattingApp.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DattingApp.API.Controllers
{

     [ApiController, Route("api/[controller]")]
     public class AuthController : ControllerBase
     {
          private readonly IAuthRepository _repo;
          private readonly IConfiguration _config;

          public AuthController(IAuthRepository repo, IConfiguration config)
          {
               this._config = config;
               _repo = repo;
          }

          [HttpPost("Register")]
          public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
          {

               userRegisterDTO.UserName = userRegisterDTO.UserName.ToLower();

               if (await _repo.UserExists(userRegisterDTO.UserName)) return BadRequest("User Already Exists");

               var UserToCreate = new User()
               {
                    UserName = userRegisterDTO.UserName
               };

               var CreatedUser = await _repo.Register(UserToCreate, userRegisterDTO.Password);

               return StatusCode(201);

          }

          [HttpPost("Login")]
          public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
          {
              // throw new Exception("Computer say no");

               var userFromRepo = await _repo.Login(userLoginDTO.UserName.ToLower(), userLoginDTO.Password);

               if (userFromRepo == null) return Unauthorized();

               var claims = new[]{
                  new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                  new Claim(ClaimTypes.Name,userFromRepo.UserName)
              };

               var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
               var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
               var tokenDescriptior=new SecurityTokenDescriptor(){
                   Subject=new ClaimsIdentity(claims),
                   Expires=DateTime.Now.AddDays(1),
                   SigningCredentials=creds
               };

               var tokenHandler=new JwtSecurityTokenHandler();

               var token=tokenHandler.CreateToken(tokenDescriptior);

               return Ok(new {token=tokenHandler.WriteToken(token)});

          }

     }
}