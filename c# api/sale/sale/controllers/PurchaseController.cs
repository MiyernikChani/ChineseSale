
using ChneseSaleApi.buisness;
using ChneseSaleApi.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sale.buisness;
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
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _service;
        private readonly ILogger<PurchaseController> _logger;
        public PurchaseController(IPurchaseService service, ILogger<PurchaseController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //get purchases by gift id
        [Route("api/Gift/getPurchasesByGiftId/{giftId}")]
        [HttpGet]
        public async Task<IActionResult> getPurchasesByGiftId(int giftId)
        {
            try
            {
                return await _service.getPurchasesByGiftId(giftId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getPurchasesByGiftId for GiftId: {giftId}, Message: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while fetching purchases by gift ID.");
            }
        }

        //buyers details
        [Route("api/Gift/getBuyersDetails")]
        [HttpGet]
        public async Task<IActionResult> getBuyersDetails()
        {
            try
            {
                return await _service.getBuyersDetails();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getBuyersDetails: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while fetching buyers details.");
            }
        }
    }
}
