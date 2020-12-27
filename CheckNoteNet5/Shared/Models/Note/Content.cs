using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models
{
    public partial class Note
    {
        [Table("Contents")]
        public class ContentDto
        {
            [Key]
            public int? NoteId { get; init; }
            [Required]
            public string Text { get; set; }
        }
    }
}
