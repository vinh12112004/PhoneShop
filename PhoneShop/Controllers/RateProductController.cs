using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model;
using PhoneShop.Services;
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
        [HttpPost]
        [Route("Create", Name = "CreateRateProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateRateProduct(RateProductDTO dto)
        {

            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                if (dto == null )
                    return BadRequest("RateProduct data is null");
                var result =await _rateProductService.AddOrUpdateRateProductAsync(userId, dto);
                _apiResponse.Data = result;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                if (_apiResponse.Errors == null) _apiResponse.Errors = new List<string>();
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
        [HttpPut]
        [Route("Update", Name = "update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateRateProduct(RateProductDTO dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                if (userId == null)
                    return Unauthorized("UserId claim is missing");
                if (dto == null)
                    return BadRequest("RateProduct data is null");
                var result = await _rateProductService.AddOrUpdateRateProductAsync(userId, dto);
                _apiResponse.Data = result;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors ??= new List<string>();
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
        [HttpGet]
        [Route("All", Name = "GetAllRateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRateProduct()
        {
            try
            {
                var rateProducts = await _rateProductService.GetRateProduct();

                _apiResponse.Data = _mapper.Map<List<RateProductDTO>>(rateProducts);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }

        }
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
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var rateProduct = await _rateProductService.GetRateProductByIdAsync(id);
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
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
            // BadRequest - 400 - client error

        }
        [HttpDelete]
        [Route("Delete/{id}", Name = "DeleteRateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteRateProduct(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);
                if (id <= 0)
                    return BadRequest();
                var result = await _rateProductService.RemoveRateProductAsync(userId, id);
                _apiResponse.Data = result;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
    }
}
