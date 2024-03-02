using EcomApi.Helper;
using EcomApi.Services.Models;
using EcomApi.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly IOrderService _orderService;
        public CustomerController(IOrderService orderService) { 

            this._orderService = orderService;

        }

        [HttpPost]
        public IActionResult GetMostRecentOrder([FromBody] RequestModel request)
        {
            try
            {
                var result = _orderService.GetMostRecentOrder(request.User, request.CustomerId);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No recent orders found for the provided customer email and ID.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing the request: {ex.Message}");
            }
        }
    }
}
