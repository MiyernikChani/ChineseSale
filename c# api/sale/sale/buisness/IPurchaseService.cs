using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace sale.buisness
{
    public interface IPurchaseService
    {
        Task<IActionResult> getPurchasesByGiftId(int giftId);
        Task<IActionResult> getBuyersDetails();
        Task<IEnumerable<Purchase>> getAllPurchases();
    }
}
