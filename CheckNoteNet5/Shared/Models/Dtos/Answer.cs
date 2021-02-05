using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    [Table("Answers")]
    [AutoMap(typeof(AnswerModel))]
    public class Answer
    {
        public int Id { get; init; }
        [Required]
        public string Text { get; init; }
        public bool? Correct { get; init; }
        [Required]
        public int QuestionId { get; init; }
        public virtual Question Question { get; init; }
    }
}
