using PayPal;
using PayPal.Api;

namespace sale.repositories;
public class PayPalRepository : IPayPalRepository
{
    private readonly string clientId = "Ab1xknDPBwceouYh4FkuvPy6TGWO21tXmChpGt_maZKV6eYVvOkNFxNPKPEgmbfqFVW2P4ilcAgJIsD-";
    private readonly string clientSecret = "EISZzNiYWRT3abIcCEU9GHt66b8Dvu3GZhgI9HxFB424atXLiCnQ3y5i6_bW2CAQNMhJr6fqcyujMXiR";

    public async Task<bool> ProcessPayment(Payment payment)
    {
        var apiContext = new APIContext(new OAuthTokenCredential(clientId, clientSecret).GetAccessToken());
        var createdPayment = payment.Create(apiContext);
        return createdPayment.state.ToLower() == "approved";
    }
}
