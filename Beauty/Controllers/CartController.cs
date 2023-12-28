using Beauty.Repositories;
using Beauty.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beauty.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;

        private readonly ILogger<CartController> _logger;

        public CartController(ICartRepository cartRepo, ILogger<CartController> logger)
        {
            _cartRepo = cartRepo;
            _logger = logger;
        }
        public async Task<IActionResult> AddItem(int itemId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(itemId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var cartCount = await _cartRepo.RemoveItem(itemId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }



        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartRepo.GetUserCart();
            var paymentInfo = new PaymentInfoViewModel(); // Инициализируйте модель платежа

            var checkoutViewModel = new CheckoutViewModel
            {
                ShoppingCart = cart,
                PaymentInfo = paymentInfo
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Ваши действия при успешной оплате
                bool isCheckedOut = await _cartRepo.DoCheckout();
                if (isCheckedOut)
                {
                    // Оплата прошла успешно, выполните дополнительные действия (например, отправка уведомления на почту)
                    return RedirectToAction("PaymentSuccess");
                }
                else
                {
                    // Ошибка при оплате
                    ModelState.AddModelError(string.Empty, "Payment failed. Please try again.");
                }
            }

            // Если ModelState.IsValid == false, возвращаем представление снова
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder()
        {
            try
            {
                bool isOrderCancelled = await _cartRepo.CancelOrder();
                if (isOrderCancelled)
                {
                    return RedirectToAction("OrderCancel");
                }
                else
                {
                    return RedirectToAction("PaymentSuccess");
                }

              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during order cancellation.");
                // Добавьте обработку ошибок, если необходимо
                return RedirectToAction("Index", "Home"); // Или другое перенаправление при ошибке
            }
        }




        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }
}
