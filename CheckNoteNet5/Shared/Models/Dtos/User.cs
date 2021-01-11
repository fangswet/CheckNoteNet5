using CheckNoteNet5.Shared.Models.Inputs;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<Note> Notes { get; }
        public virtual ICollection<Course> Likes { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public User()
        { }

        public User(RegisterInput input)
        {
            UserName = input.UserName;
            Email = input.Email;
        }
    }
}
