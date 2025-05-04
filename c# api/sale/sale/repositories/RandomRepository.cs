using ChneseSaleApi.Controllers;
using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace sale.repositories
{
    public class RandomRepository:IRandomRepository
    {
        private readonly saleDbContext _context;
        private readonly ILogger<RandomController> _logger;
        public RandomRepository(saleDbContext context, ILogger<RandomController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //get all winners
        public async Task<IEnumerable<Winner>> getAllWinners()
        {
            try
            {
                IEnumerable<Winner> winners = await _context.Winners.Include(w => w.Gift).Include(w => w.Customer).ToListAsync();
                return winners;
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in get all winners, message: {ex.Message}");   
                return Enumerable.Empty<Winner>();
            }
        }

        public async Task<IActionResult> addWinner(Winner winner)
        {
            try
            {
                _context.Winners.Add(winner);
                _context.SaveChanges();
                return new OkObjectResult("winner added successfully!" + winner);
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in adding winner id: {winner.CustomerId}, message: {ex.Message}");
                return new BadRequestObjectResult($"error in adding winner id: {winner.CustomerId}, message: {ex.Message}");
            }
        }

        public async Task<IActionResult> MakeAllGiftsAvailable()
        {
            try
            {
                IEnumerable<Gift> gifts = await _context.Gifts.ToListAsync();
                foreach (var gift in gifts)
                {
                    gift.Status = true;
                }
                await _context.SaveChangesAsync();

                IEnumerable<Winner> winners = await _context.Winners.ToListAsync();
                foreach (Winner win in winners)
                {
                    _context.Winners.Remove(win);
                }
                await _context.SaveChangesAsync();

                return new OkObjectResult("All gifts are available now");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MakeAllGiftsAvailable{ex.Message}");
                return new BadRequestObjectResult($"Error in MakeAllGiftsAvailable{ex.Message}");
            }
        }
    }
}
