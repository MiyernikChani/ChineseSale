using ChneseSaleApi.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sale.buisness;
using System.Security.Claims;

namespace sale.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService service, ILogger<CustomerController> logger)
        {
            _service = service;
            _logger = logger;
        }

        //get all gift and filter by price or category
        [HttpGet("getAllGifts")]
        public async Task<IActionResult> getAllGifts(string filterBy, string value)
        {
            try
            {
                if (await _service.isRandomed() == true)
                    return await getAllWinners();

                return await _service.getAllGifts(filterBy, value);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(getAllGifts)}: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching gifts.");
            }
        }

        //get shoping cart
        [HttpGet("getShoppingCart")]
        public async Task<IActionResult> getShoppingCart()
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(id);
                return Ok(await _service.getShoppingCart(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(getShoppingCart)}: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the shopping cart.");
            }
        }


        //add gift to cart
        [Authorize(Roles = "User")]
        [HttpPost("addGiftToCart")]
        public async Task<IActionResult> addGiftToCart(int giftId, int ammount)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(id);
                return await _service.addGiftToCart(giftId, userId, ammount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(addGiftToCart)}: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the gift to the cart.");
            }
        }

        //total price at the cart
        [HttpGet("totalPrice")]
        public async Task<IActionResult> totalPrice()
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(id);
                return await _service.totalPrice(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(totalPrice)}: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while calculating the total price.");
            }
        }


        //get all the shopping history
        [HttpGet("shoppingHistory")]
        public async Task<IActionResult> shoppingHistory()
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(id);
                return await _service.shoppingHistory(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(shoppingHistory)}: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the shopping history.");
            }
        }

        [HttpGet("getAllWinners")]
        public async Task<IActionResult> getAllWinners()
        {
            try
            {
                return await _service.getAllWinners();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(getAllWinners)}: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the winners.");
            }
        }

        //buy the gifts in the cart
        [HttpPut("buyCart")]
        public async Task<IActionResult> buyCart()
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(id);
                return await _service.buyCart(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(buyCart)}: {ex.Message}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while completing the purchase.");
            }
        }
    }
}
