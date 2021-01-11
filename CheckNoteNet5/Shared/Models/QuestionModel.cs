using CheckNoteNet5.Shared.Models.Dtos;
using System.Collections.Generic;

namespace CheckNoteNet5.Shared.Models
{
    public class QuestionModel
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public NoteEntry Note { get; init; }
        public string Text { get; init; }
        public QuestionType Type { get; init; }
        public QuestionDifficulty Difficulty { get; init; }
    }

    public class BinaryQuestionModel : QuestionModel
    {
        public bool Correct { get; init; }
    }

    public class UnaryQuestionModel : QuestionModel
    {
        public ICollection<AnswerModel> Answers { get; init; }
    }
}
