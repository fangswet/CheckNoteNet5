using AutoMapper;
using CheckNoteNet5.Shared.Models.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models.Inputs
{
    [AutoMap(typeof(Question))]
    public class QuestionInput
    {
        [Required]
        [MinLength(10)]
        public string Title { get; set; } // could be init?
        public string Text { get; set; }
        public QuestionType Type { get; init; } = QuestionType.Binary;
        public QuestionDifficulty Difficulty { get; init; } = QuestionDifficulty.Easy;
        public bool Correct { get; init; }
        public List<AnswerInput> Answers { get; } = new List<AnswerInput>();
    }
}
