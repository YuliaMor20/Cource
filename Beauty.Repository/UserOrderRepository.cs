using Beauty.Models;
using Beauty.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Beauty.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;


        public UserOrderRepository(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders = await _db.Orders
                            .Include(x=>x.OrderStatus)
                            .Include(x=>x.OrderDetail)
                            .ThenInclude(x=>x.Items)
                            .ThenInclude(x=>x.Category)
                            .Where(a=>a.UserId==userId)
                            .ToListAsync();
            return orders;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

        public async Task<IEnumerable<Order>> AllUserOrders()
        {
            // Проверяем, имеет ли текущий пользователь (вызывающий метод) право просматривать все заказы
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (currentUser == null || !await _userManager.IsInRoleAsync(currentUser, "ADMIN"))
            {
                throw new UnauthorizedAccessException("You do not have permission to view all orders.");
            }

            var orders = await _db.Orders
                            .Include(x => x.OrderStatus)
                            .Include(x => x.OrderDetail)
                            .ThenInclude(x => x.Items)
                            .ThenInclude(x => x.Category)
                            .ToListAsync();
            return orders;
        }

       
    }
}
