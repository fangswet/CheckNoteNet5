using CheckNoteNet5.Shared.Models.Auth;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckNoteNet5.Shared.Models
{
    public partial class Note
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
        public virtual User Author { get; set; }
        public virtual Note Parent { get; set; }
        public virtual ContentDto Content { get; set; } = new ContentDto();
        public virtual ICollection<Note> Children { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public class Input
        {
            [Required]
            [MinLength(5)]
            public string Title { get; set; }
            [MinLength(10)]
            public string Description { get; set; }
            [Required]
            [MinLength(25)]
            public string Text { get; set; }
            public int? ParentId { get; set; }
            public List<Question.Input> Questions { get; init; } = new List<Question.Input>();
            public List<Tag.Model> Tags { get; init; } = new List<Tag.Model>();

            public static implicit operator Note(Input i) => new Note 
            { 
                Title = i.Title, 
                Description = i.Description, 
                ParentId = i.ParentId,
                Content = new ContentDto { NoteId = null, Text = i.Text },
                Questions = i.Questions.ConvertAll(qi => (Question)qi), // does this REALLY have to be converted? who the fuck cares what class this is
                Tags = i.Tags.ConvertAll(ti => (Tag)ti)
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
            public string Text { get; init; }
        }

        // could be simplified if I used Include instead of lazy loading (i.e dont include Content)
        public class Entry
        {
            public int Id { get; init; }
            public string Title { get; init; }
            public string Description { get; init; }
            public User.Model Author { get; init; }
            public List<Tag.Model> Tags { get; init; }
        }
    }

    // ---- //
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
}
