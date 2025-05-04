using ChneseSaleApi.Controllers;
using ChneseSaleApi.models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sale.repositories;
using System.Threading;
using System.Threading.Tasks;

namespace ChneseSaleApi.repositories
{
    public class DonatorRepository:IDonatorRepository
    {
        private readonly saleDbContext _context;
        private readonly ILogger<RandomController> _logger;
        public DonatorRepository(saleDbContext context, ILogger<RandomController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //get all donators
        public async Task<IEnumerable<Donator>> GetAllDonators()
        {
            try
            {
                return await _context.Donators
                    .Include(d => d.Gifts)
                    .ThenInclude(g => g.Category)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in get all donators: {ex.Message}");
                return Enumerable.Empty<Donator>();
            }
        }

        //create donator
        public async Task<IActionResult> AddDonator(Donator donator)
        {
            try
            {
                _context.Donators.Add(donator);
                _context.SaveChanges();
                return new OkObjectResult("donator created successfully!" + donator);
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in adding donator: {ex.Message}");
                return new BadRequestObjectResult($"error in adding donator: {donator.Name}: "+ex);
            }
        }

        //delete a donator
        public async Task<IActionResult> deleteDonator(Donator donator)
        {
            try
            {
                _context.Donators.Remove(donator);
                _context.SaveChanges();
                return new OkObjectResult("donator deleted successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in adding donator: {ex.Message}");
                return new BadRequestObjectResult($"error in deleting donator id: {donator.Id}: " + ex);
            }
        }

        //update donator
        public async Task<IActionResult> updateDonator(Donator donator)
        {
            try{
                _context.Donators.Update(donator);
                _context.SaveChanges();
                return new OkObjectResult("donator " + donator.Id + " succesfully updated!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in updating donator: {ex.Message}");
                return new BadRequestObjectResult($"error in updating donator id: {donator.Id}: " + ex);
            }
        }

        //טרנזקציה - בעת הוספת מתנה, המתנה נוספת לרשימת המתנות של התורם
        public async Task<IActionResult> addGiftToDonatorById(int id, Gift gift)
        {
            try
            {
                IEnumerable<Donator> donators = await GetAllDonators();
                Donator donator = donators.FirstOrDefault(d => d.Id == id);
                donator.Gifts.Add(gift);
                await _context.SaveChangesAsync();
                return new OkObjectResult("gift added to donator id: " + id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in addGiftToDonatorById donator: {id}, gift: {gift.Id}: {ex.Message}");
                return new BadRequestObjectResult($"error in addGiftToDonatorById: " + ex);
            }
        }

        //מחיקת מתנה ספציפית מתורם ספציפי
        public async Task<IActionResult> deletGiftFromDonator(Gift gift)
        {
            try
            {
                IEnumerable<Donator> donators = await this.GetAllDonators();
                Donator donator = donators.FirstOrDefault(d => d.Id == gift.DonatorId);
                donator.Gifts.Remove(gift);
                await _context.SaveChangesAsync();
                return new OkObjectResult("gift deleted successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in deletGiftFromDonator gift: {gift.Id}: {ex.Message}");
                return new BadRequestObjectResult($"error in deletGiftFromDonator gift: {gift.Id}: {ex.Message}");
            }
        }
}
}
