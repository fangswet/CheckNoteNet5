using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckNoteNet5.Shared.Models
{
    public partial class Question
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

        public class Model
        {
            public int Id { get; init; }
            public string Title { get; init; }
            // actually supposed to be Note.Entry
            public Note.Model Note { get; init; }
            public string Text { get; init; }
            public QuestionType Type { get; init; }
            public QuestionDifficulty Difficulty { get; init; }
        }

        public class Input
        {
            [Required]
            [MinLength(10)]
            public string Title { get; set; } // could be init?
            public string Text { get; set; }
            public QuestionType Type { get; init; } = QuestionType.Binary;
            public QuestionDifficulty Difficulty { get; init; } = QuestionDifficulty.Easy;
            public bool Correct { get; init; }
            public List<Answer.Input> Answers { get; } = new List<Answer.Input>();

            public static explicit operator Question(Input i) 
                => new Question
                {
                    Title = i.Title,
                    Text = i.Text,
                    Type = i.Type,
                    Difficulty = i.Difficulty,
                    Correct = i.Correct,
                    Answers = i.Answers.ConvertAll(ai => (Answer)ai)
                };
        }

        public class BinaryModel : Model
        {
            public bool Correct { get; init; }
        }

        public class UnaryModel : Model
        {
            public ICollection<Answer.Model> Answers { get; init; }
        }

        public enum QuestionDifficulty
        {
            Easy = 1,
            Medium,
            Difficult,
            Bonus
        }
    }

    //class A
    //{
    //    static bool Answer<T>(Entity<T> ent, T ans) => ent.Answer(ans);

    //    static void Test()
    //    {
    //        var a = new BinaryEntity();

    //        Answer(a, true);
    //    }
    //}
}
