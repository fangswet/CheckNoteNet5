using AutoMapper;
using CheckNoteNet5.Shared.Models.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models.Inputs
{
    [AutoMap(typeof(Answer))]
    public class AnswerInput
    {
        [Required]
        public string Text { get; init; }
        public bool? Correct { get; init; }
    }

    public class AnswerAttempt
    {
        public int? QuestionId { get; init; }
        public bool? Correct { get; init; }
        public HashSet<int> Answers { get; init; }
        public HashSet<string> TextAnswers { get; init; }
    }
}
