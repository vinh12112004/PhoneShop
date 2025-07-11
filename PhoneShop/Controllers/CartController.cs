using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Model;
using PhoneShop.Services;
using System.Net;

namespace PhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly APIResponse _apiResponse;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            _apiResponse = new APIResponse();
        }
        [HttpPost]
        [Route("AddToCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> AddToCart([FromBody] AddToCartDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);

                var result = await _cartService.AddToCartAsync(userId, dto);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = result;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors ??= new List<string>();
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }

        }
        [HttpGet]
        [Route("GetCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var cart = await _cartService.GetCartAsync(userId);
                _apiResponse.Data = cart;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors ??= new List<string>();
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
        [HttpPost]
        [Route("RemoveFromCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> RemoveFromCart([FromBody] AddToCartDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var result = await _cartService.RemoveFromCartAsync(userId, dto);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = result;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors ??= new List<string>();
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
        [HttpDelete]
        [Route("ClearCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> ClearCart()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                var result = await _cartService.ClearCartAsync(userId);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = result;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors ??= new List<string>();
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
    }
}
