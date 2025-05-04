
using ChneseSaleApi.buisness;
using ChneseSaleApi.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sale.buisness;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChneseSaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GiftController : ControllerBase
    {
        private readonly IGiftService _service;
        private readonly ILogger<GiftController> _logger;

        public GiftController(IGiftService service, ILogger<GiftController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //get all gifts
        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<IActionResult> getAllGifts()
        {
            try
            {
                return Ok(await _service.getAllGifts());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getAllGifts: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while fetching all gifts.");
            }
        }

        //get all categories
        [Authorize(Roles = "Admin")]
        [HttpGet("getAllCategories")]
        public async Task<IActionResult> getAllCategories()
        {
            try
            {
                return Ok(await _service.getAllCategories());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getAllCategories: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while fetching all categories.");
            }
        }

        //create gift
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddGift([FromBody] Gift gift)
        {
            try
            {
                return await _service.createGift(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddGift: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while adding the gift.");
            }
        }

        //delete gift and remove from donator gift list
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteGift(int id)
        {
            try
            {
                return await _service.deleteGift(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in deleteGift for GiftId: {id}, Message: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while deleting the gift.");
            }
        }

        //update gift and update in donator gift list
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> updateGift([FromBody] Gift gift)
        {
            try
            {
                return await _service.updateGift(gift);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in updateGift: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while updating the gift.");
            }
        }

        //get donator by gift id
        [Authorize(Roles = "Admin, User")]
        [Route("getDonatorByGiftId/{id}")]
        [HttpGet]
        public async Task<IActionResult> getDonatorByGiftId(int id)
        {
            try
            {
                return await _service.getDonatorByGiftId(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getDonatorByGiftId for GiftId: {id}, Message: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while fetching the donator.");
            }
        }

        //filter gift by name\donator\countOfSales
        [Authorize(Roles = "Admin")]
        [Route("filterGift/{filterBy}")]
        [HttpGet]
        public async Task<IActionResult> filterGift(string filterBy, string value)
        {
            try
            {
                return await _service.filterGift(filterBy, value);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in filterGift with filterBy: {filterBy}, value: {value}, Message: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while filtering gifts.");
            }
        }

        //sort gifts purchase by highest price\most purchased
        [Authorize(Roles = "Admin")]
        [Route("sortPurchases/{sortBy}")]
        [HttpGet]
        public async Task<IActionResult> sortGift(string sortBy)
        {
            try
            {
                return await _service.sortGift(sortBy);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in sortGift with sortBy: {sortBy}, Message: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while sorting gifts.");
            }
        }

        [HttpPost("uploadGiftImage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadGiftImage(IFormFile file, int giftId)
        {
            try
            {
                return await _service.UploadGiftImage(file, giftId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UploadGiftImage for GiftId: {giftId}, Message: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while uploading the gift image.");
            }
        }
    }
}