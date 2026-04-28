using System.ComponentModel.DataAnnotations;

namespace AutoDrive.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        public int categoryId { get; set; }
        public Category category { get; set; } = null!;

        [Required]
        public int ModelYear { get; set; }

        [Required]
        public string Brand { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        [Required]
        public int Weight { get; set; }
        public string? Color { get; set; }

        public ICollection<LessonVehicle> lessonVehicles { get; set; } = new List<LessonVehicle>();
    }
}
