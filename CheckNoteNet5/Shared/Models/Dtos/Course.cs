using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public virtual User Author { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<User> Likes { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
    }
}
