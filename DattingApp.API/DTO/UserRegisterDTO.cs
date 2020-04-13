using System.ComponentModel.DataAnnotations;

namespace DattingApp.API.DTO
{
    public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required,StringLength(8,MinimumLength=4,ErrorMessage="your must provide 4 to 8 character long password")]
        public string Password { get; set; }
    }
}