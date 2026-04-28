namespace AutoDrive.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int enrollmentId { get; set; }
        public Enrollment enrollment { get; set; } = null!;

        public DateTime PayDate { get; set; }
        public string Method { get; set; } = null!;

        public decimal Amount { get; set; }


    }
}
