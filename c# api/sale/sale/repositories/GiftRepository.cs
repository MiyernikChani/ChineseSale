using ChneseSaleApi.Controllers;
using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sale.models;
using System.Threading;


//Services.AddDbContext<saleDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

namespace sale.repositories
{
    public class GiftRepository : IGiftRepository
    {
        private readonly saleDbContext _context;
        private readonly saleDbContext _context1;
        private readonly IDonatorRepository _donatorRepository;
        private readonly ILogger<RandomController> _logger;
        public GiftRepository(saleDbContext context, saleDbContext cotext1,IDonatorRepository donatorRepository, ILogger<RandomController> logger)
        {
            _context = context;
            _context1 = cotext1;
            _donatorRepository = donatorRepository;
            _logger = logger;
        }

        //get all gifts
        public async Task<IEnumerable<Gift>> getAllGifts()
        {
            try
            {
                return await _context.Gifts.Include(g => g.Category).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in get all gifts: {ex.Message}");
                return Enumerable.Empty<Gift>();
            }
        }

        //create gift and add to donator gifts list
        public async Task<IActionResult> createGift(Gift gift)
        {

            try
            {
                // Add gift to the context
                await _context.Gifts.AddAsync(gift);
                await _context.SaveChangesAsync();
                IEnumerable<Donator> donators = await _context1.Donators.ToListAsync();
                Donator donator = donators.FirstOrDefault(d => d.Id == gift.DonatorId);

                if (donator != null)         
                {
                    donator.Gifts.Add(gift);
                    await _context1.SaveChangesAsync();

                    var response = new
                    {
                        Message = "Gift added succeeded!",
                        Gift = gift
                    };

                    return new OkObjectResult(response);
                }
                else
                {
                    return new BadRequestObjectResult("Donator not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in createGift: {gift.Name}: {ex.Message}");
                return new BadRequestObjectResult($"error in createGift: {gift.Name}: {ex.Message}");
            }
        }

        //delete gift
        public async Task<IActionResult> deleteGift(Gift gift)
        {
            try
            {
                _context.Gifts.Remove(gift);
                await _context.SaveChangesAsync();
                return new OkObjectResult("gift deleted successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in deleteGift: {gift.Id}: {ex.Message}");
                return new BadRequestObjectResult($"error in deleteGift: {gift.Id}: {ex.Message}");
            }
        }

        //update gift
        public async Task<IActionResult> updateGift(Gift gift)
        {
            try
            {
                _context.Gifts.Update(gift);
                await _context.SaveChangesAsync();
                return new OkObjectResult("gift " + gift.Id + " succesfully updated!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in updating gift: {gift.Id}: {ex.Message}");
                return new BadRequestObjectResult($"error in updating gift: {gift.Id}: {ex.Message}");
            }
        }



        //get all categories
        public async Task<IEnumerable<Category>> getAllCategories()
        {
            try
            {
                return _context.Categories;
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in get all categories: {ex.Message}");
                return Enumerable.Empty<Category>();
            }
        }

        public async Task<int> getCount()
        {
            try
            {
                IEnumerable<Count> count = _context.Counts;
                return count.ElementAt(0).count;
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in get count: {ex.Message}");
                return 0;
            }
        }

        public async Task countPlus()
        {
            _context.Counts.ElementAt(0).count++;
            await _context.SaveChangesAsync();
        }

    }
}
