using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    [Table("Contents")]
    public class Content
    {
        [Key]
        public int? NoteId { get; init; }
        [Required]
        public string Text { get; set; }
    }
}
