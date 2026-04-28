namespace AutoDrive.Models
{
    public class Course
    {
        public int Id { get; set; }

        public int categoryId { get; set; }
        public Category category { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public ICollection<Enrollment> enrollments { get; set; } = new List<Enrollment>();
        public ICollection<CoursePlan> coursePlans { get; set; } = new List<CoursePlan>();
    }
}
