using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Model.APIResponse;
using PhoneShop.Model.User;
using PhoneShop.Services.UserService;
using System.Net;

namespace PhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;
        private readonly IUserService _userService;
        // Constructor: injects dependencies for logging, mapping, and user service
        public UserController(ILogger<UserController> logger, IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _apiResponse = new APIResponse();
            _userService = userService;
        }
        // create 1 user
        [HttpPost]
        [Route("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateUserAsync(RegisterDTO dto)
        {
            var userCreated = await _userService.CreateUser(dto);

            _apiResponse.Data = userCreated;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        // get all users
        [HttpGet]
        [Route("All", Name = "GetAllUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUser()
        {
            var users = await _userService.GetAllUsers();

            _apiResponse.Data = users;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        // get user by id
        [HttpGet]
        [Route("{id}", Name = "GetUsertById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetUserById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad request for user id={Id}", id);
                return BadRequest();
            }

            var user = await _userService.GetUserById(id);
            // NotFound - 404 - client error
            if (user == null)
            {
                _logger.LogError("user with id={Id} not found", id);
                return NotFound($"cant find user have id={id}");
            }

            _apiResponse.Data = user;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            // Ok - 200 - success
            return Ok(_apiResponse);

        }
        // update user
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<ActionResult<APIResponse>> UpdateUser([FromBody] UserUpdateDTO dto)
        {
            // check role
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (role != "Admin" && userIdClaim != dto.Id.ToString())
                return Forbid();

            // validate dto
            if (dto == null || dto.Id <= 0)
                return BadRequest();
            // call service to update user
            var result = await _userService.UpdateUser(dto);
            _apiResponse.Data = result;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);

        }
        // delete user by id
        [HttpDelete("Delete/{id}", Name = "DeleteUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> DeleteUser(int id)
        {
            // check if id is valid
            var user = await _userService.GetUserById(id);
            // NotFound - 404 - client error
            if (user == null)
            {
                _logger.LogError("user with id={Id} not found", id);
                return NotFound($"cant find user have id={id}");
            }
            // call service to delete user
            var result = await _userService.DeleteUser(id);
            _apiResponse.Data = result;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);

        }
    }
}
