using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    [AutoMap(typeof(SourceModel))]
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
    }

    public class BookSource : Source
    {
        public BookSource(string text, Note note) : base(text, SourceType.Book, note)
        { }
    }

    public class ArticleSource : Source
    {
        public ArticleSource(string text, Note note) : base(text, SourceType.Article, note)
        { }
    }

    public class VideoSource : Source
    {
        public VideoSource(string text, Note note) : base(text, SourceType.Video, note)
        { }
    }

    public enum SourceType
    {
        Book,
        Article,
        Video
    }
}
