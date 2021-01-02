using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models
{
    public partial class Question
    {
        [Table("Answers")]
        public class Answer
        {
            public int Id { get; init; }
            [Required]
            public string Text { get; init; }
            public bool? Correct { get; init; }
            [Required]
            public int QuestionId { get; init; }
            public virtual Question Question { get; init; }

            public class Model
            {
                public int Id { get; init; }
                public string Text { get; init; }
                public int QuestionId { get; init; }
            }

            public class Input
            {
                [Required]
                public string Text { get; init; }
                public bool? Correct { get; init; }

                public static explicit operator Answer(Input i) => new Answer { Text = i.Text, Correct = i.Correct };
            }
        }
    }
}
