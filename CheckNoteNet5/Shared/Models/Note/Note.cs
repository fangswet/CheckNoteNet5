using CheckNoteNet5.Shared.Models.Auth;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CheckNoteNet5.Shared.Models
{
    public partial class Note
    {
        public int Id { get; init; }
        [Required]
        public string Title { get; init; }
        public string Description { get; init; }
        [Required]
        public int ContentId { get; init; }
        [Required]
        public int AuthorId { get; set; }
        public int? ParentId { get; init; }
        public virtual User Author { get; set; }
        public virtual Note Parent { get; init; }
        public virtual ContentDto Content { get; set; }
        public virtual ICollection<Note> Children { get; init; }
        public virtual ICollection<Course> Courses { get; init; }

        public class Input
        {
            [Required]
            public string Title { get; init; }
            public string Description { get; init; }
            [Required]
            public string Text { get; init; }
            public int? ParentId { get; init; }

            public static implicit operator Note(Input i) => new Note 
            { 
                Title = i.Title, 
                Description = i.Description, 
                ParentId = i.ParentId,
                Content = new ContentDto { NoteId = null, Text = i.Text }
            };
        }

        public class Model
        {
            public int Id { get; init; }
            public string Title { get; init; }
            public string Description { get; init; }
            public User.Model Author { get; init; }
            public Model Parent { get; init; }
            public ICollection<Model> Children { get; init; }
            [JsonIgnore]
            public ContentDto Content { get; init; }
            public string Text { get => Content?.Text; }
        }
    }

    //public record Note
    //{
    //public int Id { get; }
    //[Required]
    //public string Title { get; }
    //public string Description { get; }
    //[Required]
    //public int ContentId { get; }
    //public Entity Author { get; }
    //[Required]
    //public int AuthorId { get; init; }
    //public int? ParentId { get; init; }
    //public virtual Note Parent { get; init; }
    //public virtual Content Content { get; init; }
    //public virtual List<Note> Children { get; init; }

    //    public NoteModel Model()
    //        => new NoteModel(Id, Title, Description, Parent.Model(), Content.Text, Children.Select(c => c.Model()).ToList());
    //}

    //public record NoteModel(int Id, string Title, string Description, NoteModel Parent, string Text, List<NoteModel> Children);
    // ---- //
}
