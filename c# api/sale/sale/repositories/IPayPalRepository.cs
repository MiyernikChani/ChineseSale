
using PayPal.Api;

namespace sale.repositories
{
    public interface IPayPalRepository
    {
        Task<bool> ProcessPayment(Payment payment);
    }
}
