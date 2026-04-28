namespace AutoDrive.Models
{
    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Person> people { get; set; } = new List<Person>();
    }
}
