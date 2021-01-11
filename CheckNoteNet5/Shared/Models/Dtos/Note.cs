using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    public class Note
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int ContentId { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public int? ParentId { get; set; }
        public DateTime ModifiedAt { get; set; }
        public virtual User Author { get; set; }
        public virtual Note Parent { get; set; }
        public virtual Content Content { get; set; } = new Content();
        public virtual ICollection<Note> Children { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
