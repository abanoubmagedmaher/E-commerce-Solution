using System.ComponentModel.DataAnnotations;

namespace E_commerce.Api.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Displayname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
