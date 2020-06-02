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
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DattingApp.API.Controllers
{

     [ApiController, Route("api/[controller]"),AllowAnonymous]
     public class AuthController : ControllerBase
     {
          private readonly IConfiguration _config;
          private readonly IMapper _mapper;
          private readonly UserManager<User> _userManager;
          private readonly SignInManager<User> _signInManager;

          public AuthController(IAuthRepository repo,
                                IConfiguration config,
                                IMapper mapper,
                                UserManager<User> userManager,
                                SignInManager<User> signInManager)
          {
               this._config = config;
               _mapper = mapper;
               _userManager = userManager;
               _signInManager = signInManager;
          }

          [HttpPost("Register")]
          public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
          {
               var UserToCreate = _mapper.Map<User>(userRegisterDTO);

               var result = await _userManager.CreateAsync(UserToCreate,userRegisterDTO.Password);

               var userToReturn = _mapper.Map<UserDetailedDTO>(UserToCreate);

               if(!result.Succeeded) return BadRequest(result.Errors);

               return CreatedAtRoute("GetUser", new {Controller="User", id = UserToCreate.Id},userToReturn);

          }

          [HttpPost("Login")]
          public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
          {
              // throw new Exception("Computer say no");

               var user =  await _userManager.FindByNameAsync(userLoginDTO.UserName);

               var result = await _signInManager.CheckPasswordSignInAsync(user,userLoginDTO.Password,false);

              
               if(!result.Succeeded) return Unauthorized();

               

               var appUser = _mapper.Map<UserListDetailedDTO>(user);

               return Ok(new {token=GernateJWTToken(user),user = appUser});

          }

          public string GernateJWTToken(User user){
               
               var claims = new[]{
                  new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                  new Claim(ClaimTypes.Name,user.UserName)
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

               return tokenHandler.WriteToken(token);
          }

     }
}