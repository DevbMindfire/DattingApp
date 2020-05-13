using DattingApp.API.Data.Repository;
using DattingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using System.Security.Claims;
using DattingApp.API.DTO;
using DattingApp.API.Model;
using System.Collections.Generic;
using System;

namespace DattingApp.API.Controllers
{
     [Authorize]
     [ServiceFilter(typeof(LogUserActivity))]
     [ApiController]
     [Route("api/User/{userId}/[controller]")]
     public class MessageController : ControllerBase
     {
          public IDattingRepository _repository { get; set; }
          public IMapper _mapper { get; }
          public MessageController(IDattingRepository repository, IMapper mapper)
          {
               _mapper = mapper;
               _repository = repository;

          }

          [HttpGet("{id}",Name="GetMessage")]
          public async Task<IActionResult> GetMessage(int userId , int id){

               if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               var messageFromRepo = await _repository.GetMessage(id);

               if(messageFromRepo == null) return NotFound();

               return Ok(messageFromRepo);

          }
          
          [HttpGet]
          public async Task<IActionResult> GetMessagesForUser(int userId ,[FromQuery] MessageParams messageParams){

               if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               messageParams.UserId = userId;

               var messageFromRepo = await _repository.GetMessagesForUser(messageParams);

               var messages = _mapper.Map<IEnumerable<MessagetoReturnDTO>>(messageFromRepo);

               Response.AddPagination(messageFromRepo.CurrentPage,messageFromRepo.PageSize,
                                        messageFromRepo.TotalCount,messageFromRepo.TotalPages);

               return Ok(messages);

          }

          [HttpGet("thread/{recipientId}")]
          public async Task<IActionResult> GetMessageThread(int userId, int recipientId){

               if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               var messageFromRepo = await _repository.GetMessageThread(userId,recipientId);

               var messagthread = _mapper.Map<IEnumerable<MessagetoReturnDTO>>(messageFromRepo);

               return Ok(messagthread);


          }


          [HttpPost]
          public async Task<IActionResult> CreateMessage(int userId, MessageForCreation messageForCreation){

              var sender =await _repository.GetUser(userId); 

              if(sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

              messageForCreation.SenderId = userId;

              var recipient = await _repository.GetUser(messageForCreation.RecipientId);

              if(recipient == null) return BadRequest("Could not found the User");

              var message = _mapper.Map<Message>(messageForCreation);

              _repository.Add<Message>(message);

              if(await _repository.SaveAll()){ 
                   var messageToReturn = _mapper.Map<MessagetoReturnDTO>(message);
                   return CreatedAtRoute("GetMessage",new {userId = userId , id = message.Id},messageToReturn);
               }

              throw new System.Exception("Failed in Creating Message ");


          }

          [HttpPost("{id}")]
          public async Task<IActionResult> DeleteMessage(int id,int userid){

               if(userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               var messageFromRepo = await _repository.GetMessage(id);

               if(messageFromRepo.SenderId == userid) messageFromRepo.SenderDeleted = true;

               if(messageFromRepo.RecipientId == userid) messageFromRepo.RecipientDeleted = true;

               if(messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted) _repository.Delete<Message>(messageFromRepo);

               if(await _repository.SaveAll()) return NoContent();

               throw new System.Exception("Error in Deleting message");

          }

          [HttpPost("{id}/Read")]
          public async Task<IActionResult> MarkMessageAsRead(int id, int userid){

               if(userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               var message = await _repository.GetMessage(id);

               if(message.RecipientId != userid) return Unauthorized();

               message.IsRead = true;

               message.DateRead = DateTime.Now;

               await _repository.SaveAll();

               return NoContent();
               

          }

     }
}