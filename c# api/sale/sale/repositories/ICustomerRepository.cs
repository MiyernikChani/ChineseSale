using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace sale.repositories
{
    public interface ICustomerRepository
    {
        Task<IActionResult> removeItem(Purchase p);
        Task<IEnumerable<Gift>> getAllGifts();
        Task<Gift> getGiftById(int id);
        Task<Customer> getUserById(int id);
        Task<IActionResult> addGiftToCart(Purchase purchase);
        Task<IEnumerable<Purchase>> getAllCartItems(int userId);
        Task<IActionResult> updateCartItem(Purchase purchase);
        Task<IActionResult> updateGiftCountOfSales(int giftId, int ammount);
        Task<IEnumerable<Purchase>> getAllShopping(int userId);
        Task<IEnumerable<Gift>> getAllGifts(int userId);
        Task<IActionResult> getAllWinners();
    }
}
