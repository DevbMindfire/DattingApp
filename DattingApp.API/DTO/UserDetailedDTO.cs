using System;
using System.Collections.Generic;
using DattingApp.API.Model;

namespace DattingApp.API.DTO
{
    public class UserDetailedDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string KnowsAs { get; set; }
        public DateTime Created { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Introduction { get; set; }
        public DateTime LastActive { get; set; }
        public string PhotoUrls { get; set; }
        public List<PhotoDetailedDTO> Photos { get; set; }

    }
}