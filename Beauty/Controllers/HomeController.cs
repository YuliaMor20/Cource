using Beauty.Models;
using Beauty.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Beauty.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ItemsIndex(string sterm = "", int categoryId = 0)
        {
            IEnumerable<Item> items = await _homeRepository.GetProducts(sterm, categoryId);
            IEnumerable<Category> categories = await _homeRepository.Categories();
       

            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Items = items,
                Categories = categories,
                STerm = sterm,
                CategoryId = categoryId,

            };

            return View(productModel);

        }
        public async Task<IActionResult> BServicesIndex(string sterm = "", int typeserviceId = 0)
        {
            IEnumerable<BService> bservices  = await _homeRepository.GetBServices(sterm, typeserviceId);
            IEnumerable<TypeService> typeservices = await _homeRepository.TypeServices();


            BServiceDisplayModel bserviceModel = new BServiceDisplayModel
            {
                BServices = bservices,
                TypeServices = typeservices,
                STerm = sterm,
                TypeServiceId = typeserviceId,
            };

            return View(bserviceModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}