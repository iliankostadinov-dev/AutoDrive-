using AutoDrive.Data;
using AutoDrive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly AutoDriveContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(
            AutoDriveContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ================= PROFILE =================
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var user = await _context.Users
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.PersonId == null)
                return RedirectToAction("Login", "Account");

            var roles = await _userManager.GetRolesAsync(user);

            var personId = user.PersonId.Value;

            // ================= ADMIN =================
            if (roles.Contains("Admin"))
            {
                ViewBag.Users = await _context.people.ToListAsync();
                ViewBag.Courses = await _context.courses.Include(c => c.category).ToListAsync();
                ViewBag.Categories = await _context.categories.ToListAsync();
                ViewBag.Vehicles = await _context.vehicles.Include(v => v.category).ToListAsync();
                ViewBag.LessonTypes = await _context.lessonTypes.ToListAsync();
                ViewBag.LessonStatuses = await _context.lessonStatuses.ToListAsync();
                ViewBag.LessonsPlans = await _context.lessonsPlans.Include(lp => lp.lessonType).ToListAsync();

                return View("AdminProfile");
            }

            // ================= TRAINER =================
            if (roles.Contains("Trainer"))
            {
                ViewBag.Enrollments = await _context.enrollments
                    .Include(e => e.trained)
                    .ToListAsync();

                ViewBag.Lessons = await _context.lessons
                    .Include(l => l.enrollment)
                        .ThenInclude(e => e.trained)
                    .Include(l => l.lessonType)
                    .Include(l => l.status)
                    .Include(l => l.trainer)
                    .ToListAsync();

                ViewBag.Payments = await _context.payments
                    .Include(p => p.enrollment)
                        .ThenInclude(e => e.trained)
                    .Include(p => p.enrollment)
                        .ThenInclude(e => e.course)
                            .ThenInclude(c => c.category)
                    .ToListAsync();

                ViewBag.Vehicles = await _context.vehicles.ToListAsync();
                ViewBag.LessonStatuses = await _context.lessonStatuses.ToListAsync();
                ViewBag.LessonTypes = await _context.lessonTypes.ToListAsync();

                return View("TrainerProfile");
            }

            // ================= STUDENT =================
            if (roles.Contains("Student"))
            {
                ViewBag.Enrollments = await _context.enrollments
                    .Include(e => e.course)
                        .ThenInclude(c => c.category)
                    .Where(e => e.trainedId == personId)
                    .ToListAsync();

                ViewBag.Lessons = await _context.lessons
                    .Include(l => l.lessonType)
                    .Include(l => l.status)
                    .Include(l => l.trainer)
                    .ToListAsync();

                ViewBag.Courses = await _context.courses
                    .Include(c => c.category)
                    .ToListAsync();

                ViewBag.CoursePlan = await _context.coursePlans
                    .Include(cp => cp.lessonsPlan)
                        .ThenInclude(lp => lp.lessonType)
                    .Include(cp => cp.course)
                    .ToListAsync();

                ViewBag.Payments = await _context.payments
                    .Include(p => p.enrollment)
                    .Where(p => p.enrollment.trainedId == personId)
                    .ToListAsync();

                return View("TrainedProfile");
            }

            return View();
        }

        // ================= CREATE LESSON =================
        [HttpPost]
        public async Task<IActionResult> CreateLesson(
            int enrollmentId,
            int lessonTypeId,
            int statusId,
            int? vehicleId,
            DateTime date,
            TimeSpan start,
            TimeSpan end)
        {
            var userId = _userManager.GetUserId(User);

            var user = await _context.Users
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.PersonId == null)
                return RedirectToAction("Login", "Account");

            var lesson = new Lesson
            {
                enrollmentId = enrollmentId,
                lessonTypeId = lessonTypeId,
                statusId = statusId,
                trainerId = user.PersonId.Value,
                LDate = DateTime.SpecifyKind(date, DateTimeKind.Utc),
                StartTime = start,
                EndTime = end
            };

            _context.lessons.Add(lesson);
            await _context.SaveChangesAsync();

            if (lessonTypeId == 2 && vehicleId.HasValue)
            {
                _context.lessonVehicles.Add(new LessonVehicle
                {
                    lessonId = lesson.Id,
                    vehicleId = vehicleId.Value
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditLesson(
    int id,
    int enrollmentId,
    int lessonTypeId,
    int statusId,
    DateTime date,
    TimeSpan start,
    TimeSpan end)
        {
            var lesson = await _context.lessons.FindAsync(id);

            if (lesson == null)
                return RedirectToAction("Index");

            lesson.enrollmentId = enrollmentId;
            lesson.lessonTypeId = lessonTypeId;
            lesson.statusId = statusId;
            lesson.LDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            lesson.StartTime = start;
            lesson.EndTime = end;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // ================= PAYMENT =================
        [HttpPost]
        public async Task<IActionResult> AddPayment(int enrollmentId, decimal amount, string method)
        {
            _context.payments.Add(new Payment
            {
                enrollmentId = enrollmentId,
                Amount = amount,
                Method = method,
                PayDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // ================= FREE VEHICLES =================
        [HttpGet]
        public async Task<IActionResult> GetFreeVehicles(DateTime date, TimeSpan start, TimeSpan end)
        {
            var targetDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);

            var busyVehicles = await _context.lessonVehicles
                .Include(lv => lv.lesson)
                .Where(lv =>
                    lv.lesson.LDate >= targetDate &&
                    lv.lesson.LDate < targetDate.AddDays(1))
                .Select(lv => lv.vehicleId)
                .ToListAsync();

            var freeVehicles = await _context.vehicles
                .Where(v => !busyVehicles.Contains(v.Id))
                .Select(v => new
                {
                    id = v.Id,
                    brand = v.Brand,
                    model = v.Model,
                    year = v.ModelYear
                })
                .ToListAsync();

            return Json(freeVehicles);
        }

        // ================= ENROLL COURSE =================
        [HttpPost]
        public async Task<IActionResult> EnrollCourse(int courseId)
        {
            var userId = _userManager.GetUserId(User);

            var user = await _context.Users
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.PersonId == null)
                return RedirectToAction("Index");

            var personId = user.PersonId.Value;

            var exists = await _context.enrollments
                .AnyAsync(e => e.trainedId == personId && e.courseId == courseId);

            if (!exists)
            {
                _context.enrollments.Add(new Enrollment
                {
                    trainedId = personId,
                    courseId = courseId,
                    StartDate = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}