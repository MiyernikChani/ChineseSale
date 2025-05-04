using Azure.Core;
using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sale.controllers;
using sale.repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sale.buisness
{
    public class AuthService : ControllerBase, IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthService(IAuthRepository repository, IJwtTokenService jwtTokenService, ILogger<AuthController> logger)
        {
            _repository = repository;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        //register
        public async Task<IActionResult> register(Customer customer)
        {
            try
            {
                IEnumerable<Customer> customers = _repository.getAllCustomers();
                Customer exsistCustomer = customers.FirstOrDefault(u => u.Mail == customer.Mail
                || u.FirstName.Equals(customer.FirstName) && u.LastName.Equals(customer.LastName));

                if (exsistCustomer != null)
                    return Conflict("Customer already exists.");

                Customer newCustomer = new Customer();
                newCustomer.FirstName = customer.FirstName;
                newCustomer.LastName = customer.LastName;
                newCustomer.Mail = customer.Mail;
                newCustomer.Phone = customer.Phone;
                newCustomer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);
                newCustomer.Address = customer.Address;
                newCustomer.Role = customer.Role;

                return await _repository.addCustomer(newCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in register: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        //login 
        public async Task<IActionResult> login(string firstName, string lastName, string password)
        {
            try
            {
                IEnumerable<Customer> customers = _repository.getAllCustomers();
                Customer customer = customers.FirstOrDefault(c => c.FirstName.Equals(firstName) && c.LastName.Equals(lastName));
                if (customer == null || !BCrypt.Net.BCrypt.Verify(password, customer.Password))
                    return Unauthorized("User not authorized");
                var role = "";

                var roles = new List<string>();
                if (customer.Role == "Admin")
                {
                    roles.Add("Admin");
                    roles.Add("User");
                    role = "Admin";
                }
                else
                {
                    roles.Add("User");
                    role = "User";
                }

                // Generate JWT Token
                var token = _jwtTokenService.GenerateJwtToken(firstName + " " + lastName, customer.Id.ToString(), roles);

                return Ok(new { Token = token, Role = role });
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in login: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
