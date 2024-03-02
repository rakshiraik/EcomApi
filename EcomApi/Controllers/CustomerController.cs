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
        public CustomerController(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        [HttpPost]
        public IActionResult GetMostRecentOrder([FromBody] RequestModel request)
        {
            try
            {
                var customer = _orderService.GetCustomerById(request.CustomerId);

                if (customer != null)
                {
                    if (customer.Email != request.User)
                    {
                        return NotFound("Invalid request: CustomerId and Email not belongs to same user");
                    }
                    else
                    {
                        var result = _orderService.GetMostRecentOrder(request.User, request.CustomerId);
                        return Ok(result);
                    }
                }
                else
                {
                    return NotFound("CustomerId not found");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing the request: {ex.Message}");
            }
        }
    }
}
