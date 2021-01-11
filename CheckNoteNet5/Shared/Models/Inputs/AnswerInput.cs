using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckNoteNet5.Shared.Models.Inputs
{
    public class AnswerInput
    {
        [Required]
        public string Text { get; init; }
        public bool? Correct { get; init; }
    }
}
