using Beauty.Models;
using Beauty.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beauty.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return await _db.Categories.ToListAsync();
        }
        public async Task<IEnumerable<TypeService>> TypeServices()
        {
            return await _db.TypeServices.ToListAsync();
        }
        public async Task<IEnumerable<Item>> GetProducts(string sTerm, int categoryId)
        {
            // Проверяем, не является ли sTerm null перед вызовом ToLower()
            sTerm = sTerm?.ToLower() ?? "";

            var query = from product in _db.Items
                        join category in _db.Categories on product.CategoryId equals category.Id
                        where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.Title.ToLower().StartsWith(sTerm))
                        select new Item
                        {
                            Id = product.Id,
                            Image = product.Image,
                            Title = product.Title,
                            CategoryId = product.CategoryId, 
                            Price = product.Price,
                            Category = new Category { Title = category.Title },
                          
                        };

            if (categoryId > 0)
            {
                query = query.Where(a => a.CategoryId == categoryId);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<BService>> GetBServices(string sTerm, int typeserviceId) 
        {
            // Проверяем, не является ли sTerm null перед вызовом ToLower()
            sTerm = sTerm?.ToLower() ?? "";

            var query = from bService in _db.BServices
                        join typeservice in _db.TypeServices on bService.TypeServiceId equals typeservice.Id
                        where string.IsNullOrWhiteSpace(sTerm) || (bService != null && bService.Title.ToLower().StartsWith(sTerm))
                        select new BService
                        {
                            Id = bService.Id,
                            Image = bService.Image,
                            Title = bService.Title,
                            TypeServiceId = bService.TypeServiceId,
                            Price = bService.Price,
                            TypeService = new TypeService { Title = typeservice.Title },

                        };

            if (typeserviceId > 0)
            {
                query = query.Where(a => a.TypeServiceId == typeserviceId);
            }

            return await query.ToListAsync();
        }
    }
}