using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beauty.Controllers
{

    public class SiteManagementController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {
                // Пользователь является администратором
                return View();
            }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }
        }
    }
}
