using AutoMapper;
using CheckNoteNet5.Shared.Models.Dtos;

namespace CheckNoteNet5.Shared.Models
{
    [AutoMap(typeof(Tag))]
    public class TagModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}
