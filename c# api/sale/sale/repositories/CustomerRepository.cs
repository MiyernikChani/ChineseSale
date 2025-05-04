using ChneseSaleApi.Controllers;
using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace sale.repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly saleDbContext _context;
        private readonly ILogger<RandomController> _logger;
        public CustomerRepository(saleDbContext context, ILogger<RandomController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> removeItem(Purchase p)
        {
            try
            {
                _context.Purchases.Remove(p);
                await _context.SaveChangesAsync();
                return new OkObjectResult("purchase deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in removeItem: {p.Id}, Message: {ex.Message}", ex);
                return new BadRequestObjectResult(ex);
            }
        }
        //get all gifts
        public async Task<IEnumerable<Gift>> getAllGifts()
        {
            try
            {
                IEnumerable<Gift> gifts = await _context.Gifts.Include(g => g.Category).ToListAsync();
                return gifts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in getAllGifts, Message: {ex.Message}", ex);
                throw new Exception("Error getting gifts: " + ex.Message);
            }
        }

        //get gift by id
        public async Task<Gift> getGiftById(int id)
        {
            try
            {
                IEnumerable<Gift> gifts = await getAllGifts();
                Gift gift = gifts.FirstOrDefault(g => g.Id == id);
                if (gift == null)
                    throw new Exception("There is no gift with id = " + id);
                return gift;
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in getGiftById, Message: {ex.Message}", ex);
                throw new Exception($"Error getting gift by id: {id}: " + ex.Message);
            }
        }

        //get user by id
        public async Task<Customer> getUserById(int id)
        {
            try
            {
                IEnumerable<Customer> customers = await _context.Customers.ToListAsync();
                Customer customer = customers.FirstOrDefault(c => c.Id == id);
                if (customer == null)
                    throw new Exception("There is no customer with id = " + id);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in getUserById, Message: {ex.Message}", ex);
                throw new Exception($"Error getting customer by id: {id}: " + ex.Message);
            }
        }

        //add gift to cart
        public async Task<IActionResult> addGiftToCart(Purchase purchase)
        {
            try
            {
                if (purchase == null)
                    return new BadRequestObjectResult("There is no customer with id = " + purchase.Id);
                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Gift added to cart successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in addGiftToCart, Message: {ex.Message}", ex);
                return new BadRequestObjectResult("Error adding gift to cart: " + ex.Message);
            }
        }

        //get all cart items by user id
        public async Task<IEnumerable<Purchase>> getAllCartItems(int userId)
        {
            try
            {
                IEnumerable<Purchase> purchases = await _context.Purchases.Where(p => p.CustomerId == userId && p.Status == false).Include(P=>P.Gift).Include(p=>p.Customer).ToListAsync();
                return purchases;
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in addGiftToCart, Message: {ex.Message}", ex);
                throw new Exception($"Error getting cart items user id:{userId} : " + ex.Message);
            }
        }

        //update cart item
        public async Task<IActionResult> updateCartItem(Purchase purchase)
        {
            try
            {
                _context.Purchases.Update(purchase);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Cart item updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in updateCartItem: {purchase.Id}, Message: {ex.Message}", ex);
                return new BadRequestObjectResult("Error updating cart item: " + ex.Message);
            }
        }

        //update gift count of sales
        public async Task<IActionResult> updateGiftCountOfSales(int giftId, int ammount)
        {
            try
            {
                Gift gift = await getGiftById(giftId);
                gift.CountOfSales += ammount;
                _context.Gifts.Update(gift);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Gift count of sales updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in updateGiftCountOfSales gift: {giftId}, Message: {ex.Message}", ex);
                return new BadRequestObjectResult("Error updating gift count of sales: " + ex.Message);
            }
        }

        //get all shopping history by user id
        public async Task<IEnumerable<Purchase>> getAllShopping(int userId)
        {
            try
            {
                IEnumerable<Purchase> purchases = await _context.Purchases.Where(p => p.CustomerId == userId && p.Status == true).Include(P => P.Gift).ToListAsync();
                return purchases;
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in getAllShopping for user id: {userId}:, Message: {ex.Message}", ex);
                throw new Exception("Error getting shopping history: " + ex.Message);
            }
        }

        //is randomed
        public async Task<IEnumerable<Gift>> getAllGifts(int userId)
        {
            try
            {
                return await _context.Gifts.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in getAllGifts:, Message: {ex.Message}", ex);
                return Enumerable.Empty<Gift>();
            }
        }

        //get all winners and gifts
        public async Task<IActionResult> getAllWinners()
        {
            try
            {
                IEnumerable<Winner> winners = await _context.Winners.Include(c => c.Customer).Include(c => c.Gift).ToListAsync();
                return new OkObjectResult(winners);
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in getAllGifts:, Message: {ex.Message}", ex);
                throw new Exception("Error Error in getAllGifts: " + ex.Message);
            }
        }
    }
}
