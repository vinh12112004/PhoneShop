using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Model.APIResponse;
using PhoneShop.Model.Order;
using PhoneShop.Services.OrderService;
using System.Net;

namespace PhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _cartService;
        private readonly APIResponse _apiResponse;
        // inject dependencies
        public OrderController(IOrderService cartService)
        {
            _cartService = cartService;
            _apiResponse = new APIResponse();
        }
        /// create 1 order
        [HttpPost]
        [Route("CreateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] CreateOrderDTO dto)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            var result = await _cartService.CreateOrder(userId, dto);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Data = result;
            return Ok(_apiResponse);

        }
        // get all orders of current user
        [HttpGet]
        [Route("GetOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetOrder()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var orders = await _cartService.GetOrder(userId);
            _apiResponse.Data = orders;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        
    }
}
