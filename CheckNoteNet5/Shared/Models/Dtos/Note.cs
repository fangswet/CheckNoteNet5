using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    [AutoMap(typeof(NoteEntry))]
    public class Note
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        [Required]
        public int ContentId { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public int? ParentId { get; set; }
        public int? RootId { get; set; }
        public bool IsRoot { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual User Author { get; set; }
        public virtual Note Parent { get; set; }
        public virtual Note Root { get; set; }
        public virtual Content Content { get; set; } = new Content();
        [InverseProperty(nameof(Parent))]
        public virtual ICollection<Note> Children { get; set; }
        [InverseProperty(nameof(Root))]
        public virtual ICollection<Note> AllChildren { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        [InverseProperty(nameof(Question.Note))]
        public virtual ICollection<Question> Questions { get; set; }
        [InverseProperty(nameof(Question.Root))]
        public virtual ICollection<Question> AllQuestions { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Snippet> Snippets { get; set; }
        public virtual ICollection<User> Likes { get; set; }
    }
}
