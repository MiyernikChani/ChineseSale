using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sale.buisness;

namespace sale.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly IPayPalService _service;
        public PayPalController(IPayPalService service)
        {
            _service = service;
        }

        [HttpPost("execute")]
        public async Task<IActionResult> ExecutePayment([FromBody] PaymentExecuteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.ExecutePayment(request.PaymentId, request.PayerId);
            if (result)
                return Ok("Payment Successful");
            else
                return BadRequest("Payment Failed");
        }

    // PaymentExecuteRequest.cs
    public class PaymentExecuteRequest
    {
        public string PaymentId { get; set; }
        public string PayerId { get; set; }
    }
    }
}
