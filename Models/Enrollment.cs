namespace AutoDrive.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int trainedId { get; set; }
        public Person trained { get; set; } = null!;

        public int? courseId { get; set; }
        public Course? course { get; set; }

        public DateTime StartDate { get; set; }

        public ICollection<Payment> payments { get; set; } = new List<Payment>();
        public ICollection<Lesson> lessons { get; set; } = new List<Lesson>();
    }
}
