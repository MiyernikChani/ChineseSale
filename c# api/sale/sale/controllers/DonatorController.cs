
using ChneseSaleApi.buisness;
using ChneseSaleApi.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChneseSaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DonatorController : ControllerBase
    {
        private readonly IDonatorService _service;
        private readonly ILogger<DonatorController> _logger;

        public DonatorController(IDonatorService service, ILogger<DonatorController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //get all donators
        [HttpGet]
        public async Task<IActionResult> GetAllDonators()
        {
            try
            {
                return Ok(await _service.GetAllDonators());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllDonators: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while fetching all donators.");
            }
        }

        //create donator
        [HttpPost]
        public async Task<IActionResult> AddDonator([FromBody] Donator donator)
        {
            try
            {
                return await _service.AddDonator(donator);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in AddDonator: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while adding the donator.");
            }
        }

        //delete a donator and delete donator gifts
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteDonator(int id)
        {
            try
            {
                return await _service.deleteDonator(id);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in DeleteDonator for DonatorId: {id}, Message: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while deleting the donator.");
            }
        }

        //update donator
        [HttpPut]
        public async Task<IActionResult> updateDonator([FromBody] Donator donator)
        {
            try
            {
                return await _service.updateDonator(donator);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in UpdateDonator: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while updating the donator.");
            }
        }

        //filter donator by name\mail\gift
        [HttpGet("{filterBy}")]
        public async Task<IActionResult> filterDonator(string filterBy, string value)
        {
            try
            {
                return await _service.filterDonator(filterBy, value);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in FilterDonator with filterBy: {filterBy}, value: {value}, Message: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while filtering donators.");
            }
        }
    }
}
