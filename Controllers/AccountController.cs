using AutoDrive.Data;
using AutoDrive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Controllers
{
    public class AccountController : Controller
    {
        private readonly AutoDriveContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            AutoDriveContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Register()
        {
            LoadDropdowns();

            ViewBag.IsFirstUser = !await _userManager.Users.AnyAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            Person model,
            string email,
            string password,
            int? SelectedCourseId)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(model);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // ================= 1. CHECK FIRST USER =================
                var isFirstUser = !await _userManager.Users.AnyAsync();

                var roleName = isFirstUser ? "Admin" : "Student";

                // ================= 2. PERSON =================
                model.BirthDate = DateTime.SpecifyKind(model.BirthDate, DateTimeKind.Utc);

                _context.people.Add(model);
                await _context.SaveChangesAsync();

                // ================= 3. USER =================
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = model.PhoneNumber,
                    PersonId = model.Id
                };

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    LoadDropdowns();
                    ModelState.AddModelError("", "User create failed");
                    return View(model);
                }

                await _userManager.AddToRoleAsync(user, roleName);

                // ================= 4. ENROLLMENT (ONLY STUDENTS) =================
                if (roleName == "Student" && SelectedCourseId.HasValue)
                {
                    _context.enrollments.Add(new Enrollment
                    {
                        trainedId = model.Id,
                        courseId = SelectedCourseId.Value,
                        StartDate = DateTime.UtcNow
                    });

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Profile");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                LoadDropdowns();
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RegisterUser()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(
            Person model,
            int roleId,
            int? courseId,
            string email,
            string password)
        {
            LoadDropdowns();

            if (!ModelState.IsValid)
                return View(model);

            var roleName = roleId switch
            {
                1 => "Admin",
                2 => "Trainer",
                3 => "Student",
                _ => null
            };

            if (roleName == null)
            {
                ModelState.AddModelError("", "Invalid role");
                return View(model);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // ================= PERSON =================
                model.BirthDate = DateTime.SpecifyKind(model.BirthDate, DateTimeKind.Utc);

                _context.people.Add(model);
                await _context.SaveChangesAsync();

                // ================= USER =================
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = model.PhoneNumber,
                    PersonId = model.Id
                };

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "User create failed");
                    return View(model);
                }

                await _userManager.AddToRoleAsync(user, roleName);

                // ================= ENROLLMENT =================
                if (roleName == "Student" && courseId.HasValue)
                {
                    _context.enrollments.Add(new Enrollment
                    {
                        trainedId = model.Id,
                        courseId = courseId.Value,
                        StartDate = DateTime.UtcNow
                    });

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return roleName switch
                {
                    "Admin" => RedirectToAction("Index", "AdminProfile"),
                    "Trainer" => RedirectToAction("Index", "Profile"),
                    _ => RedirectToAction("Index", "Profile")
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                LoadDropdowns();
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // ================= LOGIN FIXED =================
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError("", "Login error");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                password,
                false,
                false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Login error");
                return View();
            }

            return RedirectToAction("Index", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private void LoadDropdowns()
        {
            ViewBag.Genders = _context.genders.ToList();
            ViewBag.Countries = _context.countries.ToList();
            ViewBag.Courses = _context.courses
                .Include(c => c.category)
                .ToList();
        }
    }
}