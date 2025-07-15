using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model;
using System.Net;

namespace PhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        private readonly IPhoneShopRepository<Product> _productRepository;
        public ProductController(IMapper mapper, IPhoneShopRepository<Product> productRepository)
        {
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _productRepository = productRepository;
        }
        [HttpGet]
        [Route("All", Name = "GetAllProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();

                _apiResponse.Data = _mapper.Map<List<ProductDTO>>(products);
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
        [Route("{id}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetProductById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var product = await _productRepository.GetAsync(p => p.Id == id);
                // NotFound - 404 - client error
                if (product == null)
                {
                    return NotFound($"cant find product have id={id}");
                }

                _apiResponse.Data = _mapper.Map<ProductDTO>(product);
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
        [HttpPost]
        [Route("SearchProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> SearchProducts([FromBody] ProductFilterDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest();
                }

                var products = await _productRepository.GetAllAsyncByFilter(p => p.Price>=dto.priceFrom && p.Price<=dto.priceTo && p.Rate>= dto.rateFrom && p.Rate <= dto.rateTo);
                // NotFound - 404 - client error
                if (products == null)
                {
                    return NotFound($"cant find product have filter");
                }

                _apiResponse.Data = _mapper.Map<List<ProductDTO>>(products);
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

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> CreateProductAsync([FromBody] ProductDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();
                Product product = _mapper.Map<Product>(dto);

                var newRecord = await _productRepository.CreateAsync(product);

                dto.Id = newRecord.Id;
                _apiResponse.Data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return CreatedAtRoute("GetProductById", new { id = dto.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }

        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> UpdateProduct([FromBody] ProductDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                    return BadRequest();
                var existingProduct = await _productRepository.GetAsync(p => p.Id == dto.Id, true);
                if (existingProduct == null)
                    return NotFound($"cant find Product have id={dto.Id}");
                var newRecord = _mapper.Map<Product>(dto);
                var id = await _productRepository.UpdateAsync(newRecord);
                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }

        }
        [HttpDelete("Delete/{id}", Name = "DeleteProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var existingStudent = await _productRepository.GetAsync(student => student.Id == id);
                if (existingStudent == null)
                {
                    return NotFound($"cant find product have id={id}");
                }
                await _productRepository.DeleteAsync(existingStudent);
                _apiResponse.Data = true;
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
