namespace AutoDrive.Models
{
    public class LessonsPlan
    {
        public int Id { get; set; }

        public int lessonTypeId { get; set; }
        public LessonType lessonType { get; set; } = null!;

        public int CountHours { get; set; }

        public ICollection<CoursePlan> coursePlans { get; set; } = new List<CoursePlan>();
    }
}
