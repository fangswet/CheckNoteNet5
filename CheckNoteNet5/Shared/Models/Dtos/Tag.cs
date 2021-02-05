using AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckNoteNet5.Shared.Models.Dtos
{
    [AutoMap(typeof(TagModel))]
    public class Tag
    {
        public int Id { get; init; }
        [Required]
        public string Name { get; init; }
        public virtual ICollection<Note> Notes { get; init; }
    }
}
