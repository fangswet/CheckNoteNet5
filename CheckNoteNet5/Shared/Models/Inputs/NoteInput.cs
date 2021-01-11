using CheckNoteNet5.Shared.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models.Inputs
{
    public class NoteInput
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
        public List<QuestionInput> Questions { get; init; } = new List<QuestionInput>();
        [MaxCount(10)]
        public List<TagModel> Tags { get; init; } = new List<TagModel>();
    }
}
