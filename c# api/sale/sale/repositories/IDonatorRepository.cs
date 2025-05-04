using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChneseSaleApi.repositories
{
    public interface IDonatorRepository
    {
        Task<IEnumerable<Donator>> GetAllDonators();
        Task<IActionResult> AddDonator(Donator donator);
        Task<IActionResult> deleteDonator(Donator donator);
        Task<IActionResult> updateDonator(Donator donator);
        Task<IActionResult> addGiftToDonatorById(int id, Gift gift);
        Task<IActionResult> deletGiftFromDonator(Gift gift);
    }
}
