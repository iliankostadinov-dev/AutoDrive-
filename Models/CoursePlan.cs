namespace AutoDrive.Models
{
    public class CoursePlan
    {
        public int courseId { get; set; }
        public Course course { get; set; } = null!;

        public int lessonsPlanId { get; set; }
        public LessonsPlan lessonsPlan { get; set; } = null!;
    }
}
