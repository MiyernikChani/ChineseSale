using ChneseSaleApi.Controllers;
using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.EntityFrameworkCore;

namespace sale.repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly saleDbContext _context;
        private readonly ILogger<RandomController> _logger;
        public PurchaseRepository(saleDbContext context, ILogger<RandomController> logger)
        {
            _context = context;
        }

        //get all purchases
        public async Task<IEnumerable<Purchase>> getAllPurchases()
        {
            try
            {
                return _context.Purchases.Where(p => p.Status == true).Include(p => p.Customer).Include(p => p.Gift).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in get all purchases: {ex.Message}");
                return Enumerable.Empty<Purchase>();
            }
        }
    }
}
