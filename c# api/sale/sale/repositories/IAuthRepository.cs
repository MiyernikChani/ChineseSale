using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace sale.repositories
{
    public interface IAuthRepository
    {
        IEnumerable<Customer> getAllCustomers();
        Task<IActionResult> addCustomer(Customer customer);
    }
}
