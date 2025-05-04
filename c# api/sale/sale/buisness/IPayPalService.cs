namespace sale.buisness
{
    public interface IPayPalService
    {
        Task<bool> ExecutePayment(string paymentId, string payerId);
    }
}
