using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace sale.buisness
{
    public interface IRandomService
    {
        Task<IEnumerable<Winner>> getAllWinners();
        Task<IActionResult> randomWin(int giftId);
        Task<IActionResult> MakeAllGiftsAvailable();
        Task<IActionResult> getTotalRevenue();
        Task<IActionResult> createWinnerReport();
        Task<IActionResult> sendMailToWinners();
    }
}
