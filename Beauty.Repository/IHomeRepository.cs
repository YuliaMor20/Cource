using Beauty.Models;

namespace Beauty
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Item>> GetProducts(string sterm, int categoryId);
        Task<IEnumerable<BService>> GetBServices(string sterm, int typeserviceId);
        Task<IEnumerable<Category>> Categories(); 
        Task<IEnumerable<TypeService>> TypeServices(); 
    }
}