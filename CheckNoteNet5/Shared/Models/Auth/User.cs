using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models.Auth
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<Note> Notes { get; }
        public virtual ICollection<Course> Likes { get; set; }
        public virtual ICollection<Course.TestResult> TestResults { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public User()
        { }

        public User(Register input)
        {
            UserName = input.UserName;
            Email = input.Email;
        }

        public class Model
        {
            public int Id { get; init; }
            public string UserName { get; init; }
            public string Email { get; init; }
        }
    }
}
