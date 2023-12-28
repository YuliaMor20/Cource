using Beauty.Models;

namespace Beauty.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
        Task<IEnumerable<Order>> AllUserOrders();
	}
}