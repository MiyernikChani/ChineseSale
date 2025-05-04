using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace sale.buisness
{
    public interface IAuthService
    {
        Task<IActionResult> register(Customer customer);
        Task<IActionResult> login(string firstName, string lastName, string password);
    }
}
