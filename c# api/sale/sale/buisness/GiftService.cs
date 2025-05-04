using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sale.controllers;
using sale.repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace sale.buisness
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepository _repository;
        private readonly IDonatorRepository _donatorRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly string _imagePath1 = "D://End Project//angular project//ChineseSale//src//assets//images/";
        private readonly string _imagePath2 = "D://End Project//angular project//ChineseSale//public/";
        private readonly ILogger<AuthController> _logger;

        public GiftService(IGiftRepository repository, IDonatorRepository donatorRepository, IPurchaseRepository purchaseRepository, ILogger<AuthController> logger)
        {
            _repository = repository;
            _donatorRepository = donatorRepository;
            _purchaseRepository = purchaseRepository;
            _logger = logger;
            if (!Directory.Exists(_imagePath1))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages"));
            }
            if (!Directory.Exists(_imagePath2))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages"));
            }
        }

        //get all gifts
        public async Task<IEnumerable<Gift>> getAllGifts()
        {
            try
            {
                return await _repository.getAllGifts();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all gifts: " + ex.Message);
                return Enumerable.Empty<Gift>();
            }
        }

        public async Task<IEnumerable<Category>> getAllCategories()
        {
            try
            {
                return await _repository.getAllCategories();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching all categories: " + ex.Message);
                return Enumerable.Empty<Category>();
            }
        }

        //create gift and add to donator gift list.
        public async Task<IActionResult> createGift(Gift gift)
        {
            try
            {
                IActionResult result1 = await existGift(gift); // Await this operation
                IActionResult result2 = await existDonatorAndCategory(gift); // Await this operation

                if (result2 is BadRequestObjectResult)
                    return result2; // Return immediately if there's an issue with the donator or category

                if (result1 is BadRequestObjectResult)
                    return result1; // Return if there's an issue with the gift existence

                // Create a new Gift if no issues were found
                Gift newGift = new Gift
                {
                    Name = gift.Name,
                    DonatorId = gift.DonatorId,
                    Price = gift.Price,
                    Picture = gift.Picture,
                    Status = true,
                    CategoryId = gift.CategoryId,
                    CountOfSales = 0
                };

                return await _repository.createGift(newGift);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating gift: {gift.Name}. Exception: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }


        //delete gift and remove from donator gift list
        public async Task<IActionResult> deleteGift(int id)
        {
            try
            {
                IEnumerable<Gift> gifts = await _repository.getAllGifts();
                Gift gift = gifts.FirstOrDefault(g => g.Id == id);
                IEnumerable<Purchase> purchases = await _purchaseRepository.getAllPurchases();

                if (purchases.Contains(purchases.FirstOrDefault(p => p.GiftId == id)))
                    return new BadRequestObjectResult("There are purchases for this gift, you can't delete it");

                if (gift == null)
                    return new BadRequestObjectResult("There is no gift id = " + id);

                return await _donatorRepository.deletGiftFromDonator(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting gift with ID: {id}. Exception: {ex.Message}");
                return new BadRequestObjectResult("Error deleting gift: " + ex.Message);
            }
        }

        //update gift and update in donator gift list
        public async Task<IActionResult> updateGift(Gift gift)
        {
            try
            {
                IEnumerable<Donator> donators = await _donatorRepository.GetAllDonators();
                IActionResult result = await existDonatorAndCategory(gift);
                IActionResult result1 = await existGift(gift);

                if (result.GetType() == typeof(BadRequestObjectResult))
                    return result;

                if (result1.GetType() == typeof(BadRequestObjectResult))
                    return result1;

                else
                {
                    IEnumerable<Gift> gifts = await _repository.getAllGifts();
                    Gift g = gifts.FirstOrDefault(g => g.Id == gift.Id);

                    if (g == null)
                        return new BadRequestObjectResult("There is no gift id = " + gift.Id);
                    else
                    {
                        g.Name = gift.Name;
                        g.DonatorId = gift.DonatorId;
                        g.Price = gift.Price;
                        g.Picture = gift.Picture;
                        g.Status = gift.Status;
                        g.CategoryId = gift.CategoryId;
                        g.CountOfSales = gift.CountOfSales;

                        foreach (Donator donator in donators)
                        {
                            if (donator.Gifts == null)
                                donator.Gifts = new List<Gift>();
                            foreach (Gift g1 in donator.Gifts)
                            {
                                if (g1.Id == g.Id)
                                    donator.Gifts.Remove(g1);
                            }
                            if (donator.Id == g.DonatorId)
                                donator.Gifts.Add(g);
                        }
                        return await _repository.updateGift(g);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating gift with ID: {gift.Id}. Exception: {ex.Message}");
                return new BadRequestObjectResult("Error updating gift: " + ex.Message);
            }
        }

        //get donator by gift id
        public async Task<IActionResult> getDonatorByGiftId(int id)
        {
            try
            {
                IEnumerable<Gift> gifts = await _repository.getAllGifts();
                Gift gift = gifts.FirstOrDefault(g => g.Id == id);

                if (gift == null)
                    return new BadRequestObjectResult("There is no gift id = " + id);

                IEnumerable<Donator> donators = await _donatorRepository.GetAllDonators();
                Donator donator = donators.FirstOrDefault(d => d.Id == gift.DonatorId);
                return new OkObjectResult(donator);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fech donator with giftID: {id}. Exception: {ex.Message}");
                return new BadRequestObjectResult("Error fetch gift's donator: " + ex.Message);
            }
        }

        //filter gifts by name\donator\countOfSales
        public async Task<IActionResult> filterGift(string filterBy, string value)
        {
            try
            {
                IEnumerable<Gift> gifts = await _repository.getAllGifts();
                IEnumerable<Donator> donators = await _donatorRepository.GetAllDonators();
                IEnumerable<Gift> g;

                switch (filterBy)
                {
                    case "name":
                        g = gifts.Where(g => g.Name.Equals(value));
                        return new OkObjectResult(g);
                    case "donator":
                        Donator d = donators.FirstOrDefault(d => d.Name.Equals(value));
                        g = gifts.Where(g => g.DonatorId == d.Id);
                        return new OkObjectResult(g);
                    case "countOfSales":
                        g = gifts.Where(g => g.CountOfSales == int.Parse(value));
                        return new OkObjectResult(g);
                    default:
                        return new BadRequestObjectResult("There is no such filter: " + filterBy);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error filter gift by {filterBy}:{value}. Exception: {ex.Message}");
                return new BadRequestObjectResult("Error filter gifts: " + ex.Message);
            }
        }


        //sort gift by highest price\most purchased
        public async Task<IActionResult> sortGift(string sortBy)
        {
            try
            {
                IEnumerable<Gift> g;
                IEnumerable<Gift> gifts = await _repository.getAllGifts();
                if (sortBy == "price")
                {
                    g = gifts.OrderByDescending(g => g.Price);
                    return new OkObjectResult(g);
                }
                else if (sortBy == "numOfPurchases")
                {
                    g = gifts.OrderByDescending(p => p.CountOfSales);
                    return new OkObjectResult(g);
                }
                else
                    return new BadRequestObjectResult("invalid sort by: " + sortBy);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sort gift by {sortBy}. Exception: {ex.Message}");
                return new BadRequestObjectResult("Error sort gifts: " + ex.Message);
            }
        }

        //upload gift image
        public async Task<IActionResult> UploadGiftImage(IFormFile file, int giftId)
        {
            try
            {
                IEnumerable<Gift> gifts = await _repository.getAllGifts();
                Gift gift = gifts.FirstOrDefault(g => g.Id == giftId);
                if (gift == null)
                    return new BadRequestObjectResult("there is no gift by id: " + giftId);
                if (file == null || file.Length == 0)
                    return new BadRequestObjectResult("No file uploaded.");

                // יצירת נתיב ייחודי לקובץ
                var fileName = Path.GetFileName(file.FileName);
                var filePath1 = Path.Combine(_imagePath1, fileName);
                var filePath2 = Path.Combine(_imagePath2, fileName);


                // שמירת הקובץ
                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = new FileStream(filePath2, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                gift.Picture = fileName;
                return await updateGift(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error upload gift image for gift id: {giftId}. Exception: {ex.Message}");
                return new BadRequestObjectResult("Error upload gift image: " + ex.Message);
            }
        }


        private async Task<IActionResult> existDonatorAndCategory(Gift gift)
        {
            try
            {
                IEnumerable<Donator> donators = await _donatorRepository.GetAllDonators();
                Donator d = donators.FirstOrDefault(d => d.Id == gift.DonatorId);
                if (d == null)
                    return new BadRequestObjectResult("no such donator: " + gift.DonatorId);
                IEnumerable<Category> categories = await _repository.getAllCategories();
                Category c = categories.FirstOrDefault(c => c.Id == gift.CategoryId);
                if (c == null)
                    return new BadRequestObjectResult("no such category: " + gift.CategoryId);
                return new OkObjectResult("donator and category exist");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error validating donator and category for gift: {gift.Name}. Exception: {ex.Message}");
                return new BadRequestObjectResult("Validation error: " + ex.Message);
            }
        }

        private async Task<IActionResult> existGift(Gift gift)
        {
            try
            {
                IEnumerable<Gift> gifts = await _repository.getAllGifts();
                Gift g = gifts.FirstOrDefault(g => g.Name.Equals(gift.Name) && g.Id != gift.Id);
                if (g != null)
                    return new BadRequestObjectResult("is alrady exist gift: " + gift.Name);
                return new OkObjectResult("good");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking exist gift by gift id: {gift.Id}");
                return new BadRequestObjectResult("Error checking exist gift: " + ex.Message);
            }
        }
    }
}
