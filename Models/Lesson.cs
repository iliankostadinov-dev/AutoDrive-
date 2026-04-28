namespace AutoDrive.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        public int trainerId { get; set; }
        public Person trainer { get; set; } = null!;

        public int enrollmentId { get; set; }
        public Enrollment enrollment { get; set; } = null!;

        public int lessonTypeId { get; set; }
        public LessonType lessonType { get; set; } = null!;

        public int statusId { get; set; }
        public LessonStatus status { get; set;} = null!;

        public DateTime LDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public ICollection<LessonVehicle> lessonVehicles { get; set; } = new List<LessonVehicle>();
    }
}
