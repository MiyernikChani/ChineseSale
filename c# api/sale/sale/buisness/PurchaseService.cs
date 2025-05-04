using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sale.controllers;
using sale.repositories;

namespace sale.buisness
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;
        private readonly IGiftRepository _giftRepository;
        private readonly ILogger<AuthController> _logger;
        public PurchaseService(IPurchaseRepository repository, IGiftRepository giftRepository, ILogger<AuthController> logger)
        {
            _repository = repository;
            _giftRepository = giftRepository;
            _logger = logger;
        }

        //get all purchases
        public async Task<IEnumerable<Purchase>> getAllPurchases()
        {
            try
            {
                return await _repository.getAllPurchases();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getAllPurchases: {ex.Message}");
                return Enumerable.Empty<Purchase>();
            }
        }

        //get all purchases by gift id
        public async Task<IActionResult> getPurchasesByGiftId(int giftId)
        {
            try
            {
                IEnumerable<Gift> gifts = await _giftRepository.getAllGifts();
                Gift gift = gifts.FirstOrDefault(g => g.Id == giftId);
                if (gift == null)
                    return new NotFoundObjectResult("There is no gift id = " + giftId);
                IEnumerable<Purchase> purchases = await _repository.getAllPurchases();
                IEnumerable<Purchase> purchaseById = purchases.Where(p => p.GiftId == giftId);
                if (purchaseById.IsNullOrEmpty())
                    return new NotFoundObjectResult("No purchases found for this gift: " + giftId);
                return new OkObjectResult(purchaseById);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in getPurchasesByGiftId: {giftId}: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //get customers details
        public async Task<IActionResult> getBuyersDetails()
        {
            try
            {
                IEnumerable<Purchase> purchases = await _repository.getAllPurchases();
                IEnumerable<Customer> buyers = purchases.Select(p => p.Customer).Distinct();
                return new OkObjectResult(buyers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getBuyersDetails: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
