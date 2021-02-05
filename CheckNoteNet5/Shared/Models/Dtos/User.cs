using AutoMapper;
using CheckNoteNet5.Shared.Models.Inputs;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    [AutoMap(typeof(UserModel))]
    public class User : IdentityUser<int>
    {
        [InverseProperty(nameof(Note.Author))]
        public virtual ICollection<Note> Notes { get; }
        [InverseProperty(nameof(Course.Likes))]
        public virtual ICollection<Course> CourseLikes { get; set; }
        [InverseProperty(nameof(Note.Likes))]
        public virtual ICollection<Note> NoteLikes { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        [InverseProperty(nameof(Course.Author))]
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
