using AutoMapper;
using CheckNoteNet5.Shared.Models.Dtos;

namespace CheckNoteNet5.Shared.Models
{
    [AutoMap(typeof(Answer))]
    public class AnswerModel
    {
        public int Id { get; init; }
        public string Text { get; init; }
        public int QuestionId { get; init; }
    }
}
