using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;
using sale.repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using sale.controllers;

namespace sale.buisness
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private int userId;
        private readonly ILogger<AuthController> _logger;

        public CustomerService(ICustomerRepository repository, ILogger<AuthController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //put user id
        public void putUserId(int id)
        {
            userId = id;
            Console.WriteLine("user id: " + userId);
        }

        //get all gift
        public async Task<IActionResult> getAllGifts(string filterBy, string value)
        {
            try
            {
                IEnumerable<Gift> gifts = await _repository.getAllGifts();
                IEnumerable<Gift> filteredGifts = null;
                switch (filterBy)
                {
                    case "price":
                        filteredGifts = gifts.Where(g => g.Price <= Convert.ToInt32(value));
                        return new OkObjectResult(filteredGifts);
                    case "category":
                        filteredGifts = gifts.Where(g => g.Category.Equals(value));
                        return new OkObjectResult(filteredGifts);
                    default:
                        return new OkObjectResult(gifts);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getAllGifts: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public async Task<IEnumerable<Purchase>> getShoppingCart(int userId)
        {
            try
            {
                return await _repository.getAllCartItems(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getShoppingCart: {ex.Message}");
                return Enumerable.Empty<Purchase>();
            }
        }

        //add gift to cart
        public async Task<IActionResult> addGiftToCart(int giftId, int userId, int ammount)
        {
            try
            {
                IEnumerable<Purchase> cartItems = await _repository.getAllCartItems(userId);
                Gift gift = await _repository.getGiftById(giftId);
                Customer user = await _repository.getUserById(userId);
                if (gift == null)
                    return new BadRequestObjectResult("no such gift: " + giftId);
                if (user == null)
                    return new BadRequestObjectResult("no such user: " + userId);

                Purchase purchase = new Purchase();
                purchase.GiftId = giftId;
                purchase.CustomerId = userId;
                purchase.Status = false;
                purchase.Ammount = ammount;
                purchase.TotalPrice = gift.Price * ammount;

                Purchase p = cartItems.FirstOrDefault(p => p.GiftId == giftId);
                if (p != null)
                {
                    p.Ammount += ammount;
                    if (p.Ammount <= 0)
                    {
                        await _repository.removeItem(p);
                    }
                    p.TotalPrice += gift.Price * ammount;
                    return new OkObjectResult(await _repository.updateCartItem(p));
                }
                return new OkObjectResult(await _repository.addGiftToCart(purchase));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in addGiftToCart: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //total price at the cart
        public async Task<IActionResult> totalPrice(int userId)
        {
            try
            {
                IEnumerable<Purchase> cartItems = await _repository.getAllCartItems(userId);
                double totalPrice = 0;
                foreach (Purchase p in cartItems)
                {
                    totalPrice += p.TotalPrice;
                }
                return new OkObjectResult(totalPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in totalPrice: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //buy the gifts in the cart
        public async Task<IActionResult> buyCart(int userId)
        {
            try
            {
                IEnumerable<Purchase> cartItems = await _repository.getAllCartItems(userId);
                foreach (Purchase p in cartItems)
                {
                    p.Status = true;
                    await _repository.updateCartItem(p);
                    await _repository.updateGiftCountOfSales(p.GiftId, p.Ammount);
                }
                return new OkObjectResult("Cart bought successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in buyCart: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //get all the shopping history
        public async Task<IActionResult> shoppingHistory(int userId)
        {
            try
            {
                IEnumerable<Purchase> purchases = await _repository.getAllShopping(userId);
                return new OkObjectResult(purchases);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in shoppingHistory: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //is randomed
        public async Task<bool> isRandomed()
        {
            try
            {
                IEnumerable<Gift> gifts = await _repository.getAllGifts();
                foreach (Gift g in gifts)
                {
                    if (g.Status == true)
                        return false;
                }
                return true;
            }
            catch(Exception ex) {
                _logger.LogError($"Error in isRandomed: {ex.Message}");
                return false; 
            }
        }

        //get all winners and gifts
        public async Task<IActionResult> getAllWinners()
        {
            try
            {
                return await _repository.getAllWinners();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getAllWinners: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
