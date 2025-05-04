using ChneseSaleApi.Controllers;
using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace sale.repositories
{
    public class AuthRepository:IAuthRepository
    {
        private readonly saleDbContext _context;
        private readonly ILogger<RandomController> _logger;

        public AuthRepository(saleDbContext context, ILogger<RandomController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //get all customers
        public IEnumerable<Customer> getAllCustomers()
        {
            try
            {
                return _context.Customers;
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in getAllCustomers, Message: {ex.Message}", ex);
                return Enumerable.Empty<Customer>();
            }
        }

        //add customer
        public async Task<IActionResult> addCustomer(Customer customer)
        {
            try
            {
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Customer registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"repository: Error in addCustomer, Message: {ex.Message}", ex);
                return new BadRequestObjectResult("Error registering customer: " + ex.Message);
            }
        }

    }
}
