namespace AutoDrive.Models
{
    public class LessonVehicle
    {
        public int lessonId {  get; set; }
        public Lesson lesson { get; set; } = null!;

        public int vehicleId { get; set; }
        public Vehicle vehicle { get; set; } = null!;
    }
}
