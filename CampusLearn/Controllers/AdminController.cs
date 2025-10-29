using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CampusLearn.Data;
using CampusLearn.Models;
using CampusLearn.Models.ViewModels;

namespace CampusLearn.Controllers
{
    public class AdminController : Controller
    {
        private readonly CampusLearnContext _db;
        private readonly PasswordHasher<Admin> _hasher = new();

        public AdminController(CampusLearnContext db) => _db = db;

        // ---------- Auth ----------
        [HttpGet]
        public IActionResult Login() => View(new AdminLoginVm());

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginVm vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);

            var admin = await _db.Admins.FirstOrDefaultAsync(a => a.AdminEmail == vm.Email);
            if (admin == null)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View(vm);
            }

            var verification = _hasher.VerifyHashedPassword(admin, admin.AdminPasswordHash, vm.Password);
            if (verification == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.AdminID.ToString()),
                new Claim(ClaimTypes.Name, $"{admin.AdminName} {admin.AdminSurname}"),
                new Claim(ClaimTypes.Email, admin.AdminEmail),
                new Claim("IsSuperAdmin", admin.IsSuperAdmin ? "True" : "False")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = vm.RememberMe });

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult Denied() => View();

        // ---------- Dashboard ----------
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var meId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var me = await _db.Admins.FindAsync(meId);
            ViewBag.Me = me;
            return View();
        }

        // ---------- Admin Management (SuperAdmin only) ----------
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> Index()
        {
            var list = await _db.Admins.AsNoTracking().OrderBy(a => a.AdminName).ToListAsync();
            return View(list);
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpGet]
        public IActionResult Create() => View(new AdminCreateVm());

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create(AdminCreateVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (await _db.Admins.AnyAsync(a => a.AdminEmail == vm.AdminEmail))
            {
                ModelState.AddModelError(nameof(vm.AdminEmail), "Email already exists.");
                return View(vm);
            }

            var admin = new Admin
            {
                AdminName = vm.AdminName,
                AdminSurname = vm.AdminSurname,
                AdminPhone = vm.AdminPhone,
                AdminEmail = vm.AdminEmail,
                IsSuperAdmin = vm.IsSuperAdmin
            };
            admin.AdminPasswordHash = _hasher.HashPassword(admin, vm.Password);

            _db.Admins.Add(admin);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var a = await _db.Admins.FindAsync(id);
            if (a == null) return NotFound();

            var vm = new AdminEditVm
            {
                AdminID = a.AdminID,
                AdminName = a.AdminName,
                AdminSurname = a.AdminSurname,
                AdminPhone = a.AdminPhone,
                AdminEmail = a.AdminEmail,
                IsSuperAdmin = a.IsSuperAdmin
            };
            return View(vm);
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Edit(AdminEditVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var a = await _db.Admins.FindAsync(vm.AdminID);
            if (a == null) return NotFound();

            // unique email check
            var emailTaken = await _db.Admins
                .AnyAsync(x => x.AdminEmail == vm.AdminEmail && x.AdminID != vm.AdminID);
            if (emailTaken)
            {
                ModelState.AddModelError(nameof(vm.AdminEmail), "Email already in use.");
                return View(vm);
            }

            a.AdminName = vm.AdminName;
            a.AdminSurname = vm.AdminSurname;
            a.AdminPhone = vm.AdminPhone;
            a.AdminEmail = vm.AdminEmail;
            a.IsSuperAdmin = vm.IsSuperAdmin;

            if (!string.IsNullOrWhiteSpace(vm.NewPassword))
            {
                a.AdminPasswordHash = _hasher.HashPassword(a, vm.NewPassword);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.Admins.FindAsync(id);
            if (a == null) return NotFound();

            // don’t allow deleting the last superadmin
            if (a.IsSuperAdmin)
            {
                var others = await _db.Admins.CountAsync(x => x.IsSuperAdmin && x.AdminID != id);
                if (others == 0)
                {
                    TempData["Error"] = "Cannot delete the last SuperAdmin.";
                    return RedirectToAction(nameof(Index));
                }
            }

            _db.Admins.Remove(a);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

