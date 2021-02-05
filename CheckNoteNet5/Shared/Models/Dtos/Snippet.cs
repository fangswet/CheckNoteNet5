using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    public class Snippet
    {
        public int Id { get; set; }
        [Required]
        public int NoteId { get; set; }
        public virtual Note Note { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
