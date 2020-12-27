using Microsoft.AspNetCore.Identity;

namespace CheckNoteNet5.Shared.Models.Auth
{
    public class Role : IdentityRole<int>
    {
        public const string Admin = "admin";
    }
}
