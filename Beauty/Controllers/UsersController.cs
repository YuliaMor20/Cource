using Beauty.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UsersController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Index()

    {
        if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
        {
            var users = _userManager.Users.ToList();
        return View(users);
        }
        else
        {
            // Пользователь не является администратором
            return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
        }
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(string userId)
    {
        if (User.IsInRole("ADMIN"))
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            var model = new EditUserRoleViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles
            };

            return View("~/Views/Users/EditRole.cshtml", model);
        }
        else
        {
            // Пользователь не является администратором
            return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
        }
    }


    [HttpPost]
    public async Task<IActionResult> EditRole(EditUserRoleViewModel model)
    {
        if (User.IsInRole("ADMIN"))
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

        if (user == null)
        {
            return NotFound();
        }

        // Удаление текущих ролей пользователя
        var userRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, userRoles);

        // Добавление новых ролей
        await _userManager.AddToRolesAsync(user, model.SelectedRoles);

        return RedirectToAction("Index"); }
          else
        {
            // Пользователь не является администратором
            return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
        }
    }
}