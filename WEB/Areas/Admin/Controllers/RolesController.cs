using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WEB.Areas.Admin.Models;
using WEB.Models.Entities;

namespace WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager
        ) : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }

        public IActionResult CreateRole() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen aşağıdaki kurallara uyunuz!";
                return View(model);
            }

            var roleNameCheck = await _roleManager.Roles.AnyAsync(x => x.Name == model.Name);
            if (roleNameCheck)
            {
                TempData["Error"] = "Bu rol ismi kullanılmaktadır!";
                return View(model);
            }

            var resultCreate = await _roleManager.CreateAsync(new IdentityRole(model.Name));
            if (resultCreate.Succeeded)
            {
                TempData["Success"] = "Rol başarılı bir şekilde eklendi!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Rol eklenemedi!";
            return View(model);
        }

        public async Task<IActionResult> AssignToRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                TempData["Error"] = "Böyle bir role bulunamadı!";
                return RedirectToAction("Index");
            }

            var model = new AssignToRoleVM()
            {
                RoleName = roleName
            };

            foreach (var user in await _userManager.Users.ToListAsync())
            {
                var list = await _userManager.IsInRoleAsync(user, model.RoleName) ?
                    model.HasRole : model.HasNotRole;
                list.Add(user);
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToRole(AssignToRoleVM model)
        {
            bool result = true;
            foreach (var userId in model.AddIds ?? [])
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var resultAddToRole = await _userManager.AddToRoleAsync(user, model.RoleName);
                    
                    if (!resultAddToRole.Succeeded)
                        result = false;
                }
            }

            foreach (var userId in model.DeleteIds ?? [])
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var resultRemoveFromRole = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                    
                    if (!resultRemoveFromRole.Succeeded)
                        result = false;
                }
            }

            if (!result)
            {
                TempData["Error"] = "İşlem sırasında hata oluştu!";
                return View(model);
            }

            TempData["Success"] = "Rol atamaları başarılı bir şekilde gerçekleşti!";
            return RedirectToAction("Index");
        }

        //Burada da Role'ün update ve delete bölümlerini yapın.(Dikkat: O rolde kullanıcı varsa silinmemeli!)

        //Admin giriş yaptığında Admin Panelinde üstteki Rol Yönetimi sekmesinin yanında Kullanıcı Yönetimi sekmesin de olsun. Burada kullanıcılar listelesin, güncellesin ve silebilsin.

        //Giriş yapan kullanıcı admin ise Admin Paneli sayfasında iken "Anasayfaya Dön" butonu gözüksün. Anasayfadayken de "Admin Paneli" butonu gözüksün.
    }
}
