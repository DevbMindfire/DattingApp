using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DattingApp.API.Data.Repository;
using DattingApp.API.DTO;
using DattingApp.API.Helpers;
using DattingApp.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DattingApp.API.Controllers
{
     [Authorize, Route("api/User/{userId}/photos"),ApiController]
     public class PhotoController : ControllerBase
     {
          public IDattingRepository _repository { get; }
          public IMapper _mapper { get; }
          public Cloudinary _cloudnary ;
          public IOptions<CloudinarySetting> _cloudinaryConfig { get; }
          public PhotoController(IDattingRepository repository, IMapper mapper, IOptions<CloudinarySetting> cloudinaryConfig)
          {
               _cloudinaryConfig = cloudinaryConfig;
               _mapper = mapper;
               _repository = repository;

               Account acc = new Account(
                   _cloudinaryConfig.Value.CloudName,
                   _cloudinaryConfig.Value.ApiKey,
                   _cloudinaryConfig.Value.ApiSecret
               );

               _cloudnary = new Cloudinary(acc);

          }

          [HttpGet("{id}", Name="GetPhoto")]
          public async Task<IActionResult> GetPhoto(int id){
              
              var photoFromRepo =await _repository.GetPhoto(id);

              var photo = _mapper.Map<PhotoForReturnDTO>(photoFromRepo);

              return Ok(photo);

          }

          [HttpPost]
          public async Task<IActionResult> AddPhotoForUser(int userId,[FromForm]PhotoForCreationDto photoForCreation){
              
              if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               var user = await _repository.GetUser(userId);

               var file = photoForCreation.File;
               
               var uploadResult = new ImageUploadResult();

               if(file.Length > 0){

                   using(var stream = file.OpenReadStream()){

                       var uploadParams = new ImageUploadParams(){
                           File = new FileDescription(file.Name,stream),
                           Transformation = new Transformation().
                                Width(500).Height(500).Crop("fill").Gravity("face")
                       };

                       uploadResult = _cloudnary.Upload(uploadParams);
                       
                   }

               }

                photoForCreation.Url = uploadResult.Uri.ToString();
                photoForCreation.PublicId = uploadResult.PublicId;

                var photo = _mapper.Map<Photo>(photoForCreation);

                
                photo.UserId = user.Id;
                photo.IsMain = true;
                if(user.Photos.Any(use => use.IsMain))
                    photo.IsMain = false;

                _repository.Add<Photo>(photo);
                if(await _repository.SaveAll()){
                    var photoToreturn = _mapper.Map<PhotoForReturnDTO>(photo);
                    return CreatedAtRoute("GetPhoto",new { userId = userId, id = photo.Id },photoToreturn);
                }

                return BadRequest("Could not add photo");

          }

          [HttpPost("{id}/setMain")]
          public async Task<IActionResult> SetMainPhoto(int userId , int id){

               if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               var user = await _repository.GetUser(userId);

               if(!user.Photos.Any(p => p.Id == id)) return Unauthorized();

               var photoFromRepo =await _repository.GetPhoto(id);

               if(photoFromRepo.IsMain) return BadRequest("This is already the main photo");

               var currentMainPhoto =await _repository.GetMainPhotoFromUser(userId);

               currentMainPhoto.IsMain = false;

               photoFromRepo.IsMain = true;
            
               if(await _repository.SaveAll()) return NoContent();

               return BadRequest("Could not set your Photo to main");

          }
          [HttpDelete("{id}")]
          public async Task<IActionResult> DeletePhoto(int userId,int id){
               
               if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

               var user = await _repository.GetUser(userId);

               if(!user.Photos.Any(p => p.Id == id)) return Unauthorized();

               var photoFromRepo =await _repository.GetPhoto(id);

               if(photoFromRepo.IsMain) return BadRequest("You cannot delete your main photo");

               if(photoFromRepo.PublicId != null){
                    DeletionParams deletion = new DeletionParams(photoFromRepo.PublicId);

                    var result = _cloudnary.Destroy(deletion);

                    if(result.Result == "ok") 
                        _repository.Delete(photoFromRepo);
               }
               else if(photoFromRepo.PublicId == null){
                   _repository.Delete(photoFromRepo);
               }

               if(await _repository.SaveAll())
                return Ok();

               return BadRequest("failed to delete photo");
          }
     }
}