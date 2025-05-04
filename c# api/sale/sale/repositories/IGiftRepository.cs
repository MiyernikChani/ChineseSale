using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace sale.repositories
{
    public interface IGiftRepository
    {
        Task<IEnumerable<Gift>> getAllGifts();
        Task<IActionResult> createGift(Gift gift);
        Task<IActionResult> deleteGift(Gift gift);
        Task<IActionResult> updateGift(Gift gift);
        Task<int> getCount();
        Task countPlus();
        Task<IEnumerable<Category>> getAllCategories();
    }
}
