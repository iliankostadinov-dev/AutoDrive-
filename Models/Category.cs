namespace AutoDrive.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Vehicle> vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Course> courses { get; set; } = new List<Course>();
    }
}
