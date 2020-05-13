using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DattingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DattingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ValueController:ControllerBase
    {

        private readonly DataContext db;
        public ValueController(DataContext _db)
        {
            this.db=_db;
        }

        
        [HttpGet]
        public IActionResult Get(){
            //if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();

            return Ok(User.FindFirst(ClaimTypes.NameIdentifier).Value+" just hold on");

        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int? id){

            return Ok(db.Values.First(User=>User.Id==id));

        }
        

    }
}