using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WEB.Models.AccountViewModels;
using WEB.Models.Entities;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WEB.Controllers
{
    public class AccountController(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        IPasswordHasher<AppUser> passwordHasher
        ) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher = passwordHasher;

        #region Register
        public IActionResult Register() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            //Kayıt ol işlemleri
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen aşağıdaki kurallara uyunuz!";
                return View(model);
            }

            var emailResult = await _userManager.Users.AnyAsync(x => x.Status != Status.Passive && x.Email == model.Email);
            if (emailResult)
            {
                TempData["Error"] = "Bu email kullanılmaktadır!";
                return View(model);
            }

            var user = new AppUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthdate = model.Birthdate,
                Email = model.Email,
                UserName = model.UserName
            };

            IdentityResult resultCreate = await _userManager.CreateAsync(user, model.Password);
            if (resultCreate.Succeeded)
            {
                TempData["Success"] = "Kaydınız yapılmıştır. Giriş yapabilirsiniz!";
                return RedirectToAction("Login");
            }
            TempData["Error"] = string.Join(",\n", resultCreate.Errors.Select(x => x.Description));
            return View(model);
        }
        #endregion

        #region Login
        public IActionResult Login(string? returnUrl)
        {
            var model = new LoginVM { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen aşağıdaki kurallara uyunuz!";
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                TempData["Error"] = "Böyle bir kullanıcı bulunamadı!";
                return View(model);
            }

            SignInResult resultSignIn = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            
            if (resultSignIn.Succeeded)
            {
                TempData["Success"] = $"{user.FirstName} {user.LastName} Hoşgeldiniz!!";
                return Redirect(model.ReturnUrl ?? "/Home/Index");
            }
         
            TempData["Error"] = "Kullanıcı adı veya şifre yanlış!";
            return View(model);
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "Çıkış Yapıldı!";
            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}
