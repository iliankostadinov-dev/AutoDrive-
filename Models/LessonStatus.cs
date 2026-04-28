namespace AutoDrive.Models
{
    public class LessonStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Lesson> lessons { get; set; } = new List<Lesson>();
    }
}
