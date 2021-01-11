using CheckNoteNet5.Shared.Models.Dtos;

namespace CheckNoteNet5.Shared.Models
{
    public class SourceModel
    {
        public string Text { get; set; }
        public string Comment { get; set; }
        public SourceType Type { get; set; }
    }
}
