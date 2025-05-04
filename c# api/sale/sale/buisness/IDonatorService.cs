using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace ChneseSaleApi.buisness
{
    public interface IDonatorService
    {
        Task<IEnumerable<Donator>> GetAllDonators();
        Task<IActionResult> AddDonator(Donator donator);
        Task<IActionResult> deleteDonator(int id);
        Task<IActionResult> updateDonator(Donator donator);
        Task<IActionResult> filterDonator(string filterBy, string value);
    }
}
