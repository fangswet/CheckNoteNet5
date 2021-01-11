using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models.Inputs
{
    public class LoginInput
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterInput : LoginInput
    {
        [Required]
        public string UserName { get; set; }
    }
}
