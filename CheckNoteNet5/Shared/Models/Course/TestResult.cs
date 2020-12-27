using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models
{
    public partial class Course
    {
        [Table("TestResults")]
        public class TestResult
        {
            public int Id { get; set; }

            [Required]
            public int CourseId { get; set; }
            public virtual Course Course { get; set; }

            [Required]
            public int UserId { get; set; }
            public virtual Auth.User User { get; set; }

            [Required]
            [Range(0, 100)]
            public int Result { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }
}
