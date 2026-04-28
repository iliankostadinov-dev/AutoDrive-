using Microsoft.AspNetCore.Identity;

namespace AutoDrive.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? PersonId { get; set; }

        public Person? Person { get; set; }
    }
}