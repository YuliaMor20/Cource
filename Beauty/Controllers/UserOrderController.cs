using Beauty.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beauty.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepo;

        public UserOrderController(IUserOrderRepository userOrderRepo)
        {
            _userOrderRepo = userOrderRepo;
        }

        public async Task<IActionResult> UserOrders()
        {
            var orders = await _userOrderRepo.UserOrders();
            return View(orders);
        }

       // Добавляем атрибут авторизации по роли ADMIN
        public async Task<IActionResult> AllUserOrders()
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("MANAGER"))
            {
                // Получаем заказы всех пользователей
                var orders = await _userOrderRepo.AllUserOrders();

            // Возвращаем представление с заказами всех пользователей
            return View(orders);
            }
            else
            {
                // Пользователь не является администратором
                return RedirectToAction("AccessDenied", "Account"); // Например, перенаправление на страницу с отказом в доступе
            }
        }
	

	}
}

