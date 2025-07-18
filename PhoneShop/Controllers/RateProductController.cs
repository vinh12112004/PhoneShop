using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model;
using PhoneShop.Model.APIResponse;
using PhoneShop.Services;
using PhoneShop.Services.RateProductService;
using System.Net;
using System.Threading.Tasks;

namespace PhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RateProductController : ControllerBase
    {
        private readonly IRateProductServicce _rateProductService;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        public RateProductController(IRateProductServicce rateProductService, IMapper mapper)
        {
            _rateProductService = rateProductService;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }
        // create rate product
        [HttpPost]
        [Route("Create", Name = "CreateRateProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateRateProduct(RateProductDTO dto)
        {

            // Get userId from claims
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            // Check if userId is null
            if (dto == null)
                return BadRequest("RateProduct data is null");
            var result = await _rateProductService.AddOrUpdateRateProductAsync(userId, dto);
            _apiResponse.Data = result;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        // Update rate product
        [HttpPut]
        [Route("Update", Name = "update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateRateProduct(RateProductDTO dto)
        {
            // Get userId from claims
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            if (userId == null)
                return Unauthorized("UserId claim is missing");
            // Check if dto is null
            if (dto == null)
                return BadRequest("RateProduct data is null");
            // Call service to add or update rate product
            var result = await _rateProductService.AddOrUpdateRateProductAsync(userId, dto);
            _apiResponse.Data = result;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        // Get all rate products
        [HttpGet]
        [Route("All", Name = "GetAllRateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRateProduct()
        {
            // Call service to get all rate products
            var rateProducts = await _rateProductService.GetRateProduct();

            _apiResponse.Data = _mapper.Map<List<RateProductDTO>>(rateProducts);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);

        }
        // Get rate product by id
        [HttpGet]
        [Route("{id}", Name = "GetRateProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetRateProductById(int id)
        {
            // Get userId from claims
            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            if (id <= 0)
            {
                return BadRequest();
            }

            var rateProduct = await _rateProductService.GetRateProductByIdAsync(userId, id);
            // NotFound - 404 - client error
            if (rateProduct == null)
            {
                return NotFound($"cant find rate product have id={id}");
            }

            _apiResponse.Data = _mapper.Map<RateProductDTO>(rateProduct);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            // Ok - 200 - success
            return Ok(_apiResponse);

        }
        // Delete rate product by id
        [HttpDelete]
        [Route("Delete/{id}", Name = "DeleteRateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteRateProduct(int id)
        {
            // Get userId from claims
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            if (id <= 0)
                return BadRequest();
            // call service to remove rate product
            var result = await _rateProductService.RemoveRateProductAsync(userId, id);
            _apiResponse.Data = result;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
    }
}
