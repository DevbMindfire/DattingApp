using System.Threading.Tasks;
using DattingApp.API.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DattingApp.API.DTO;
using System.Collections.Generic;
using DattingApp.API.Model;

namespace DattingApp.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class LodeController : ControllerBase
    {
          private readonly IDattingRepository _repository;

          public LodeController(IDattingRepository repository)
        {
               _repository = repository;
          }
        public async Task<User> Register(User user, string password)
          {
               byte[] passwrodHash,passwordSalt;
               CreatePasswordHash(password,out passwrodHash,out passwordSalt);
               user.PasswordHash=passwrodHash;
               user.PasswordSalt=passwordSalt;

                _repository.Add<User>(user);
                await _repository.SaveAll();
               
               return user;
          }
           private void CreatePasswordHash(string password, out byte[] passwrodHash, out byte[] passwordSalt)
          {
               using(var hmac=new System.Security.Cryptography.HMACSHA512()){

                   passwordSalt=hmac.Key;
                   passwrodHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

               }
          }



          [HttpPost,Route("AddUser")]
          public async Task<IActionResult> AddUser([FromBody]User user){

              var r=await Register(user,"password");

              if(user.PasswordHash !=null)
                return Ok("ok");

            return Ok("Noyt ok");
          }
    }

    
}