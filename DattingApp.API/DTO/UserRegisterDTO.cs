using System.ComponentModel.DataAnnotations;
using System;
namespace DattingApp.API.DTO
{
    public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required,StringLength(8,MinimumLength=4,ErrorMessage="your must provide 4 to 8 character long password")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Knownas { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string  Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public UserRegisterDTO()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }

    }
}