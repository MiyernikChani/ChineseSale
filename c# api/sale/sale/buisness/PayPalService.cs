using PayPal.Api;
using sale.repositories;

namespace sale.buisness
{
    public class PayPalService : IPayPalService
    {
        private readonly IPayPalRepository _repository;

        public PayPalService(IPayPalRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecutePayment(string paymentId, string payerId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };
            return await _repository.ProcessPayment(payment);
        }
    }
}
