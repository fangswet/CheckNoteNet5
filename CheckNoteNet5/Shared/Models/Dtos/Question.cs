using AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    [AutoMap(typeof(BinaryQuestionModel))]
    [AutoMap(typeof(UnaryQuestionModel))]
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int NoteId { get; set; }
        public virtual Note Note { get; set; }
        public int? RootId { get; set; }
        public virtual Note Root { get; set; }
        public string Text { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        public bool? Correct { get; set; }
        [Required]
        public QuestionDifficulty Difficulty { get; set; }
        public virtual ICollection<Answer> Answers { get; init; }
        public bool Answer(bool answer) 
            => Type == QuestionType.Binary && Correct == answer;
        public bool Answer(HashSet<int> answers) 
            => answers != null && Type == QuestionType.Binary && Answers.Select(a => a.Id).ToHashSet().SetEquals(answers);
        public bool Answer(HashSet<string> answers) 
            => answers != null && Type == QuestionType.Binary && Answers.Select(a => a.Text).ToHashSet().SetEquals(answers);

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

    public class BinaryQuestion : Question
    {
        public BinaryQuestion(string text, Note note) : base(text, QuestionType.Binary, note) { }
    }

    public class SelectQuestion : Question
    {
        public SelectQuestion(string text, Note note) : base(text, QuestionType.Select, note) { }
    }

    public class InputQuestion : Question
    {
        public InputQuestion(string text, Note note) : base(text, QuestionType.Input, note) { }
    }
}
