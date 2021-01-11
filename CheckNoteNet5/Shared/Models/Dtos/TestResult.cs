using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNoteNet5.Shared.Models.Dtos
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
        public virtual User User { get; set; }

        [Required]
        [Range(0, 100)]
        public int Result { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
