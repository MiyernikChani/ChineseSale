using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace sale.repositories
{
    public interface IRandomRepository
    {
        Task<IActionResult> addWinner(Winner winner);
        Task<IActionResult> MakeAllGiftsAvailable();
        Task<IEnumerable<Winner>> getAllWinners();

    }
}
