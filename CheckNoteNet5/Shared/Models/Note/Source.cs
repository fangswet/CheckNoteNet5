using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models
{
    public partial class Note
    {
        public class Source
        {
            public int Id { get; set; }
            [Required]
            public string Text { get; set; }
            public string Comment { get; set; }
            [Required]
            public SourceType Type { get; set; }
            [Required]
            public virtual int NoteId { get; set; }
            public virtual Note Note { get; set; }

            public Source(string text, SourceType type, Note note)
            {
                Text = text;
                Type = type;
                Note = note;
            }

            public class Model
            {
                public string Text { get; set; }
                public string Comment { get; set; }
                public SourceType Type { get; set; }
            }

            public enum SourceType
            {
                Book,
                Article,
                Video
            }

            public class Book : Source
            {
                public Book(string text, Note note) : base(text, SourceType.Book, note)
                { }
            }

            public class Article : Source
            {
                public Article(string text, Note note) : base(text, SourceType.Article, note)
                { }
            }

            public class Video : Source
            {
                public Video(string text, Note note) : base(text, SourceType.Video, note)
                { }
            }
        }
    }
}
