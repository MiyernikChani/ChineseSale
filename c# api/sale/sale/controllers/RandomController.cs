
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
    public class RandomController : ControllerBase
    {
        private readonly IRandomService _service;
        private readonly ILogger<RandomController> _logger;

        public RandomController(IRandomService service, ILogger<RandomController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //get all winners
        [Route("getAllWinners")]
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> getAllWinners()
        {
            try
            {
                return Ok(await _service.getAllWinners());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getAllWinners: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while fetching all winners.");
            }
        }

        //random win to gift by user id and gift id
        [Route("api/Random/randomWin/{giftId}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> randomWin(int giftId)
        {
            try
            {
                return await _service.randomWin(giftId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in randomWin for GiftId: {giftId}, Message: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while performing random win.");
            }
        }

        //Make all gifts available
        [Route("api/Gift/MakeAllGiftsAvailable")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeAllGiftsAvailable()
        {
            try
            {
                return await _service.MakeAllGiftsAvailable();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MakeAllGiftsAvailable: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while making all gifts available.");
            }
        }

        //Total revenue
        [Route("api/Random/getTotalRevenue")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getTotalRevenue()
        {
            try
            {
                return await _service.getTotalRevenue();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getTotalRevenue: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while fetching total revenue.");
            }
        }

        //create winner report
        [Route("api/Random/createWinnerReport")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> createWinnerReport()
        {
            try
            {
                return await _service.createWinnerReport();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in createWinnerReport: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while creating winner report.");
            }
        }

        //send mail to the winners
        [Route("api/Random/sendMailToWinners")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> sendMailToWinners()
        {
            try
            {
                return await _service.sendMailToWinners();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in sendMailToWinners: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while sending mail to winners.");
            }
        }
    }
}
