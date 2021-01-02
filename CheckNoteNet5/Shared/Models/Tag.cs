using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models
{
    public class Tag
    {
        public int Id { get; init; }
        [Required]
        public string Name { get; init; }
        public virtual ICollection<Note> Notes { get; init; }
        public virtual ICollection<Question> Questions { get; init; }

        public class Model
        {
            public int Id { get; init; }
            public string Name { get; init; }

            public static explicit operator Tag(Model m) => new Tag { Id = m.Id, Name = m.Name };
        }
    }
}
