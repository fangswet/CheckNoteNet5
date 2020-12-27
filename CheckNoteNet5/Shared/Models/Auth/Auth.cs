using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models.Auth
{
    public class Login
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class Register : Login
    {
        [Required]
        public string UserName { get; set; }
    }
}
