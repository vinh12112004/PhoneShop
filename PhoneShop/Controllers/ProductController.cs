using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model.APIResponse;
using PhoneShop.Model.Product;
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

        // Constructor: injects dependencies for mapping and repository access
        public ProductController(IMapper mapper, IPhoneShopRepository<Product> productRepository)
        {
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _productRepository = productRepository;
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        [HttpGet]
        [Route("All", Name = "GetAllProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct()
        {
            // Retrieve all products from the repository
            var products = await _productRepository.GetAsyncInclude(x => x.Include(x => x.ImageProducts));
            _apiResponse.Data = _mapper.Map<List<ProductDTO>>(products);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        /// <summary>
        /// Get a product by its ID.
        /// </summary>
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
            if (id <= 0)
            {
                return BadRequest();
            }

            // Find product by ID
            var product = await _productRepository.GetAsync(p => p.Id == id, p => p.Include(p => p.ImageProducts));
            if (product == null)
            {
                // Return 404 if not found
                return NotFound($"cant find product have id={id}");
            }

            _apiResponse.Data = _mapper.Map<ProductDTO>(product);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        /// <summary>
        /// Search products by filter (price and rate range).
        /// </summary>
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
            if (dto == null)
            {
                return BadRequest();
            }

            // Filter products by price and rate range
            var products = await _productRepository.GetAllAsyncByFilter(
                p => p.Price >= dto.priceFrom && p.Price <= dto.priceTo && p.Rate >= dto.rateFrom && p.Rate <= dto.rateTo && dto.category == p.Category
                , p => p.Include(p => p.ImageProducts)
                );
            if (products == null)
            {
                return NotFound($"cant find product have filter");
            }

            _apiResponse.Data = _mapper.Map<List<ProductDTO>>(products);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        /// <summary>
        /// Create a new product. Only accessible by Admin.
        /// </summary>
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
            if (dto == null)
                return BadRequest();

            // Map DTO to entity and create new product
            Product product = _mapper.Map<Product>(dto);
            var newRecord = await _productRepository.CreateAsync(product);

            dto.Id = newRecord.Id;
            _apiResponse.Data = dto;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            // Return 201 Created with route to new product
            return CreatedAtRoute("GetProductById", new { id = dto.Id }, _apiResponse);
        }

        /// <summary>
        /// Update an existing product. Only accessible by Admin.
        /// </summary>
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
            if (dto == null || dto.Id <= 0)
                return BadRequest();

            // Check if product exists
            var existingProduct = await _productRepository.GetAsync(p => p.Id == dto.Id, p => p.Include(p => p.ImageProducts), true);
            if (existingProduct == null)
                return NotFound($"cant find Product have id={dto.Id}");

            // Map DTO to entity and update product
            _mapper.Map(dto, existingProduct);
            var id = await _productRepository.UpdateAsync(existingProduct);
            return NoContent();
        }

        /// <summary>
        /// Delete a product by ID. Only accessible by Admin.
        /// </summary>
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
            if (id <= 0)
            {
                return BadRequest();
            }

            // Find product by ID
            var existingProduct = await _productRepository.GetAsync(student => student.Id == id);
            if (existingProduct == null)
            {
                return NotFound($"cant find product have id={id}");
            }

            // Delete product
            await _productRepository.DeleteAsync(existingProduct);
            _apiResponse.Data = true;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
    }
}