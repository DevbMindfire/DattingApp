using System.Threading.Tasks;
using DattingApp.API.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DattingApp.API.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using DattingApp.API.Helpers;

namespace DattingApp.API.Controllers
{
     
     [Authorize]
     [ApiController]
     [Route("api/[controller]")]
     public class UserController : ControllerBase
     {
          public IDattingRepository _repository { get; }
          public IMapper _mapper { get; }

          public UserController(IDattingRepository repository,IMapper mapper)
          {
               _repository = repository;
               _mapper = mapper;
          }

          [HttpGet]
          public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams){

              var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

              var userFromRepo =await _repository.GetUser(currentUserId); 

              userParams.UserId = currentUserId;

              if(string.IsNullOrEmpty(userParams.Gender)){

                   userParams.Gender = userFromRepo.Gender == "male" || userFromRepo.Gender == "Male" ? "Female" : "male" ;
               

              }

              var user = await _repository.GetUsers(userParams);

              var userList=_mapper.Map<List<UserListDetailedDTO>>(user);

              Response.AddPagination(user.CurrentPage,user.PageSize,user.TotalCount,user.TotalPages);

              return Ok(userList);  

          }

          [ServiceFilter(typeof(LogUserActivity))]
          [HttpGet("{id}",Name="GetUser")]
          public async Task<IActionResult> GetUser(int? id){

              var user=_mapper.Map<UserDetailedDTO>(await _repository.GetUser(id.Value));

              return  Ok(user);

          }
          [HttpPut("{id}")]
          public async Task<IActionResult> UpdateUser(int id,UpdateUserDTO updateUserDTO){
               
               if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               var user = await _repository.GetUser(id);

               _mapper.Map(updateUserDTO,user);

               if(await _repository.SaveAll()) return NoContent();

               throw new System.Exception($"Updating user model on {id} failed");

          }
     }
}