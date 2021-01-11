using Microsoft.AspNetCore.Identity;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    public class Role : IdentityRole<int>
    {
        public const string Admin = "admin";
    }
}
