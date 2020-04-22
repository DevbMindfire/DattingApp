using System.Threading.Tasks;
using DattingApp.API.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DattingApp.API.DTO;
using System.Collections.Generic;

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
          public async Task<IActionResult> GetUsers(){

              var userList=_mapper.Map<List<UserListDetailedDTO>>(await _repository.GetUsers());

              return Ok(userList);  

          }

          [HttpGet("{id}")]
          public async Task<IActionResult> GetUser(int? id){

              var user=_mapper.Map<UserDetailedDTO>(await _repository.GetUser(id.Value));

              return  Ok(user);

          }
     }
}