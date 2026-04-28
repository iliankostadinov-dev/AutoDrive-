using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AutoDrive.Models
{
    public class Person
    {
        public int Id { get; set; }

        public int genderId { get; set; }

        [ValidateNever]
        public Gender gender { get; set; } = null!;

        public int countryId { get; set; }

        [ValidateNever]
        public Country country { get; set; } = null!;

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public bool IsArchived { get; set; } = false;

        public DateTime? ArchivedOn { get; set; }

        public ICollection<Enrollment> enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Lesson> trainerLessons { get; set; } = new List<Lesson>(); 
    }
}
