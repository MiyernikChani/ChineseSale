using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace sale.buisness
{
    public interface IGiftService
    {
        Task<IEnumerable<Gift>> getAllGifts();
        Task<IEnumerable<Category>> getAllCategories();
        Task<IActionResult> createGift(Gift gift);
        Task<IActionResult> deleteGift(int id);
        Task<IActionResult> updateGift(Gift gift);
        Task<IActionResult> getDonatorByGiftId(int id);
        Task<IActionResult> filterGift(string filterBy, string value);
        Task<IActionResult> sortGift(string sortBy);
        Task<IActionResult> UploadGiftImage(IFormFile file, int giftId);
    }
}
