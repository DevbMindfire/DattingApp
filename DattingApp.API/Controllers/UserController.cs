using System.Threading.Tasks;
using DattingApp.API.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DattingApp.API.DTO;
using System.Collections.Generic;
using System.Security.Claims;
using DattingApp.API.Helpers;
using DattingApp.API.Model;

namespace DattingApp.API.Controllers
{
     
     [Authorize]
     [ServiceFilter(typeof(LogUserActivity))]
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

                   userParams.Gender = userFromRepo.Gender == "male" || userFromRepo.Gender == "Male" ? "female" : "male" ;

              }

              var user = await _repository.GetUsers(userParams);

              var userList=_mapper.Map<List<UserListDetailedDTO>>(user);

              Response.AddPagination(user.CurrentPage,user.PageSize,user.TotalCount,user.TotalPages);

              return Ok(userList);  

          }

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

          [HttpPost("{id}/like/{recipientId}")]
          public async Task<IActionResult> LikeUser(int id , int recipientId){

               if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               var like = await _repository.GetLike(id,recipientId);

               if(like != null) return BadRequest("You already like that user");

               if(await _repository.GetUser(recipientId) == null) return NotFound();

               like = new Like(){
                   LikeeId = recipientId,
                   LikerId = id  
               };

               _repository.Add<Like>(like);

               if(await _repository.SaveAll()) return Ok();

               return BadRequest("Failed to like user");

          }
     }
}