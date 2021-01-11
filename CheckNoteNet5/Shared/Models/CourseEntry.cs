namespace CheckNoteNet5.Shared.Models
{
    public class CourseEntry
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserModel Author { get; set; }
    }
}
