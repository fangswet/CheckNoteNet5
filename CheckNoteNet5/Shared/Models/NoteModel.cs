namespace CheckNoteNet5.Shared.Models
{
    public class NoteModel : NoteEntry
    {
        public NoteModel Parent { get; init; }
        public string Text { get; init; }
    }
}
