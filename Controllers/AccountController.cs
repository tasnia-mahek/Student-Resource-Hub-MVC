using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using student_resource_hub.Data;
using student_resource_hub.Models;
using student_resource_hub.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace student_resource_hub.Controllers
{
    public class AccountController : Controller
    {
        private static readonly string[] Roles = ["Student", "Admin", "CR"];
        private readonly ApplicationDbContext context;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IWebHostEnvironment environment;

        public AccountController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, IWebHostEnvironment environment)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
            this.environment = environment;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(int? id = null)
        {
            var ownerId = GetCurrentUserId();
            if (id is null)
            {
                id = ownerId;
            }

            var user = await context.Users.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id.Value);
            if (user is null)
            {
                return NotFound();
            }

            return View(CreateProfileViewModel(user, user.Id == ownerId));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var ownerId = GetCurrentUserId();
            if (model.Id != ownerId)
            {
                return Forbid();
            }

            var user = await context.Users.SingleOrDefaultAsync(item => item.Id == ownerId);
            if (user is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                CopyProfileData(model, user);
                model.IsOwner = true;
                return View(model);
            }

            user.FullName = model.EditedFullName.Trim();
            user.Bio = model.EditedBio.Trim();
            user.Location = model.EditedLocation.Trim();
            user.PhoneNumber = model.EditedPhoneNumber.Trim();

            if (model.ProfilePhoto is { Length: > 0 })
            {
                var extension = Path.GetExtension(model.ProfilePhoto.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.ProfilePhoto), "Use a JPG, PNG, or WebP image.");
                    CopyProfileData(model, user);
                    model.IsOwner = true;
                    return View(model);
                }

                var uploadDirectory = Path.Combine(environment.WebRootPath, "uploads", "profiles");
                Directory.CreateDirectory(uploadDirectory);
                var fileName = $"user-{user.Id}-{Guid.NewGuid():N}{extension}";
                await using var stream = System.IO.File.Create(Path.Combine(uploadDirectory, fileName));
                await model.ProfilePhoto.CopyToAsync(stream);
                user.ProfilePhotoPath = $"/uploads/profiles/{fileName}";
            }

            await context.SaveChangesAsync();
            await RefreshSignIn(user);
            TempData["ProfileMessage"] = "Your profile was updated.";
            return RedirectToAction(nameof(Profile));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim().ToLowerInvariant();
            var user = await context.Users.SingleOrDefaultAsync(item => item.Email == email);
            if (user is null || !Roles.Contains(user.Role))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            if (!string.Equals(user.Role, model.Role, StringComparison.Ordinal))
            {
                ModelState.AddModelError(nameof(model.Role), "Selected role does not match this account.");
                return View(model);
            }

            var passwordResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (passwordResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim().ToLowerInvariant();
            if (await context.Users.AnyAsync(user => user.Email == email))
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName.Trim(),
                Email = email,
                Role = model.Role
            };
            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

            try
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Your account was created. Please sign in.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private int GetCurrentUserId()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return context.Users.Where(user => user.Email == email).Select(user => (int?)user.Id).SingleOrDefault() ?? 0;
        }

        private static ProfileViewModel CreateProfileViewModel(User user, bool isOwner) => new()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            Bio = user.Bio,
            Location = user.Location,
            PhoneNumber = user.PhoneNumber,
            ProfilePhotoPath = user.ProfilePhotoPath,
            CreatedAt = user.CreatedAt,
            IsOwner = isOwner,
            EditedFullName = user.FullName,
            EditedBio = user.Bio,
            EditedLocation = user.Location,
            EditedPhoneNumber = user.PhoneNumber
        };

        private static void CopyProfileData(ProfileViewModel model, User user)
        {
            model.FullName = user.FullName;
            model.Email = user.Email;
            model.Role = user.Role;
            model.Bio = user.Bio;
            model.Location = user.Location;
            model.PhoneNumber = user.PhoneNumber;
            model.ProfilePhotoPath = user.ProfilePhotoPath;
            model.CreatedAt = user.CreatedAt;
        }

        private async Task RefreshSignIn(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }
}
