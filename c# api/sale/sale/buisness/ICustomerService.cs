using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace sale.buisness
{
    public interface ICustomerService
    {
        Task<IActionResult> getAllGifts(string filterBy, string value);
        Task<IActionResult> addGiftToCart(int giftId, int userId, int ammount);
        void putUserId(int id);
        Task<IActionResult> buyCart(int userId);
        Task<IActionResult> totalPrice(int userId);
        Task<IActionResult> shoppingHistory(int userId);
        Task<bool> isRandomed();
        Task<IActionResult> getAllWinners();
        Task<IEnumerable<Purchase>> getShoppingCart(int userId);
    }
}
