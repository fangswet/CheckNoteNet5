using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models
{
    public partial class Course
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public virtual Auth.User Author { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Auth.User> Likes { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }

        public class Entry
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public Auth.User.Model Author { get; set; }
        }
    }
}
