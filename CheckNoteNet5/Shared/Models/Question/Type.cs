using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckNoteNet5.Shared.Models
{
    public partial class Question
    {
        public enum QuestionType
        {
            Binary,
            Select,
            Input
        }
    }

    public class Question<TAnswer> : Question
    {
        public Question(string text, QuestionType type, Note note) : base(text, type, note)
        { }
        public virtual bool Answerr(TAnswer answer) => false;
    }

    public class BinaryEntity : Question<bool>
    {
        public BinaryEntity(string text, Note note) : base(text, QuestionType.Binary, note) { }
        public override bool Answerr(bool answer) => Correct == answer;
    }

    public class SelectEntity : Question<HashSet<int>>
    {
        public SelectEntity(string text, Note note) : base(text, QuestionType.Select, note) { }
        public override bool Answerr(HashSet<int> answers) => Answers.Select(a => a.Id).ToHashSet().SetEquals(answers);
    }

    public class InputEntity : Question<HashSet<string>>
    {
        public InputEntity(string text, Note note) : base(text, QuestionType.Input, note) { }
        public override bool Answerr(HashSet<string> answers) => Answers.Select(a => a.Text).ToHashSet().SetEquals(answers); // rename (fuck c#)
    }
}
