using Beauty.Models;

namespace Beauty.ViewModels
{
    public class CheckoutViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }
        public PaymentInfoViewModel PaymentInfo { get; set; }
    }
}
