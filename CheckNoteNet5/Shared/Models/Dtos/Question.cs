using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int NoteId { get; set; }
        public virtual Note Note { get; set; }
        public string Text { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        public bool? Correct { get; set; }
        [Required]
        public QuestionDifficulty Difficulty { get; set; }
        public virtual ICollection<Answer> Answers { get; init; }
        public virtual ICollection<Tag> Tags { get; init; }

        public Question()
        { }

        protected Question(string title, QuestionType type, Note note)
        {
            Title = title;
            Type = type;
            Note = note;
        }
    }

    public enum QuestionDifficulty
    {
        Easy = 1,
        Medium,
        Difficult,
        Bonus
    }

    public enum QuestionType
    {
        Binary,
        Select,
        Input
    }

    public class Question<TAnswer> : Question
    {
        public Question(string text, QuestionType type, Note note) : base(text, type, note)
        { }
        public virtual bool Answerr(TAnswer answer) => false;
    }

    public class BinaryQuestion : Question<bool>
    {
        public BinaryQuestion(string text, Note note) : base(text, QuestionType.Binary, note) { }
        public override bool Answerr(bool answer) => Correct == answer;
    }

    public class SelectQuestion : Question<HashSet<int>>
    {
        public SelectQuestion(string text, Note note) : base(text, QuestionType.Select, note) { }
        public override bool Answerr(HashSet<int> answers) => Answers.Select(a => a.Id).ToHashSet().SetEquals(answers);
    }

    public class InputQuestion : Question<HashSet<string>>
    {
        public InputQuestion(string text, Note note) : base(text, QuestionType.Input, note) { }
        public override bool Answerr(HashSet<string> answers) => Answers.Select(a => a.Text).ToHashSet().SetEquals(answers); // rename (fuck c#)
    }
}
