namespace AutoDrive.Models
{
    public class LessonType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<LessonsPlan> lessonsPlans { get; set; } = new List<LessonsPlan>();
        public ICollection<Lesson> lessons { get; set; } = new List<Lesson>();
    }
}
