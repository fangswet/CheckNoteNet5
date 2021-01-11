using System.Collections.Generic;

namespace CheckNoteNet5.Shared.Models
{
    public class NoteEntry
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public UserModel Author { get; init; }
        public List<TagModel> Tags { get; init; }
    }
}
