using AutoDrive.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Data
{
    public class AutoDriveContext : IdentityDbContext<ApplicationUser>
    {
        public AutoDriveContext(DbContextOptions<AutoDriveContext> options)
            : base(options)
        {
        }

        public DbSet<Person> people { get; set; }
        public DbSet<Gender> genders { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Vehicle> vehicles { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<Enrollment> enrollments { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<LessonType> lessonTypes { get; set; }
        public DbSet<LessonStatus> lessonStatuses { get; set; }
        public DbSet<LessonsPlan> lessonsPlans { get; set; }
        public DbSet<CoursePlan> coursePlans { get; set; }
        public DbSet<Lesson> lessons { get; set; }
        public DbSet<LessonVehicle> lessonVehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
               .HasOne(u => u.Person)
               .WithOne(p => p.ApplicationUser)
               .HasForeignKey<Person>(p => p.ApplicationUserId);


            modelBuilder.Entity<CoursePlan>().HasKey(cp => new { cp.courseId, cp.lessonsPlanId });
            modelBuilder.Entity<LessonVehicle>().HasKey(lv => new { lv.lessonId, lv.vehicleId });

            modelBuilder.Entity<Course>()
                .HasOne(c => c.category)
                .WithMany(c => c.courses)
                .HasForeignKey(c => c.categoryId);

            modelBuilder.Entity<Course>()
                .Property(c => c.TotalPrice)
                .HasPrecision(7, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(7, 2);
        }
    }
}