using AutoDrive.Data;
using AutoDrive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Controllers
{
    public class AdminController : Controller
    {
        private readonly AutoDriveContext _context;

        public AdminController(AutoDriveContext context)
        {
            _context = context;
        }

        public IActionResult AdminProfile()
        {
            ViewBag.Categories = _context.categories.ToList();

            ViewBag.Courses = _context.courses
                .Include(c => c.category)
                .ToList();

            ViewBag.Vehicles = _context.vehicles
                .Include(v => v.category)
                .ToList();

            ViewBag.LessonTypes = _context.lessonTypes.ToList();

            ViewBag.LessonStatuses = _context.lessonStatuses.ToList();

            ViewBag.LessonsPlans = _context.lessonsPlans
                .Include(lp => lp.lessonType)
                .ToList();

            ViewBag.CoursePlans = _context.coursePlans
                .Include(cp => cp.course)
                    .ThenInclude(c => c.category)
                .Include(cp => cp.lessonsPlan)
                    .ThenInclude(lp => lp.lessonType)
                .ToList();

            return View("~/Views/Profile/AdminProfile.cshtml");
        }

        [HttpPost]
        public IActionResult AddCategory(Category model)
        {
            _context.categories.Add(model);
            _context.SaveChanges();
            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult AddVehicle(Vehicle vehicle)
        {
            _context.vehicles.Add(vehicle);
            _context.SaveChanges();
            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult AddCourse(Course model)
        {
            _context.courses.Add(model);
            _context.SaveChanges();
            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult AddLessonType(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _context.lessonTypes.Add(new LessonType
                {
                    Name = name
                });

                _context.SaveChanges();
            }

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult AddLessonStatus(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _context.lessonStatuses.Add(new LessonStatus
                {
                    Name = name
                });

                _context.SaveChanges();
            }

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult AddLessonsPlan(int lessonTypeId, int countHours)
        {
            var plan = new LessonsPlan
            {
                lessonTypeId = lessonTypeId,
                CountHours = countHours
            };

            _context.lessonsPlans.Add(plan);
            _context.SaveChanges();

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult AddCoursePlan(int courseId, int lessonsPlanId)
        {
            var cp = new CoursePlan
            {
                courseId = courseId,
                lessonsPlanId = lessonsPlanId
            };

            _context.coursePlans.Add(cp);
            _context.SaveChanges();

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.categories
                .FirstOrDefault(x => x.Id == id);

            if (category != null)
            {
                _context.categories.Remove(category);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult DeleteVehicle(int id)
        {
            var vehicle = _context.vehicles
                .FirstOrDefault(x => x.Id == id);

            if (vehicle != null)
            {
                _context.vehicles.Remove(vehicle);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.courses
                .FirstOrDefault(x => x.Id == id);

            if (course != null)
            {
                _context.courses.Remove(course);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult DeleteLessonType(int id)
        {
            var item = _context.lessonTypes.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                _context.lessonTypes.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult DeleteLessonStatus(int id)
        {
            var item = _context.lessonStatuses.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                _context.lessonStatuses.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult DeleteLessonsPlan(int id)
        {
            var plan = _context.lessonsPlans.Find(id);

            if (plan != null)
            {
                _context.lessonsPlans.Remove(plan);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminProfile");
        }

        [HttpPost]
        public IActionResult DeleteCoursePlan(int courseId, int lessonsPlanId)
        {
            var cp = _context.coursePlans
                .FirstOrDefault(x =>
                    x.courseId == courseId &&
                    x.lessonsPlanId == lessonsPlanId);

            if (cp != null)
            {
                _context.coursePlans.Remove(cp);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminProfile");
        }
    }
}