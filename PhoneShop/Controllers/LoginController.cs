using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhoneShop.Model.Login;
using PhoneShop.Services.UserService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        // Inject configuration and user service dependencies
        public LoginController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="model">Login credentials (username and password)</param>
        /// <returns>JWT token and user info if successful, error otherwise</returns>
        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO model)
        {
            // Validate input model
            if (!ModelState.IsValid)
            {
                return BadRequest("pls provide password and username");
            }

            // Retrieve user by username
            var user = await _userService.GetUserByUsername(model.Username);

            // Prepare response DTO
            LoginResponseDTO reponse = new()
            {
                Username = model.Username,
                RoleName = user.RoleName
            };

            // Check password validity
            if (!CheckPassword(model.Password, user.Password))
            {
                return BadRequest("Invalid username or password");
            }

            // Generate JWT token
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecret"));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, user.RoleName),
                    new Claim("UserId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            reponse.Token = tokenHandler.WriteToken(token);

            // Return token and user info
            return Ok(reponse);
        }

        /// <summary>
        /// Verifies the input password against the saved hashed password.
        /// </summary>
        /// <param name="inputPassword">Password provided by user</param>
        /// <param name="savedPassword">Stored password in "hash.salt" format</param>
        /// <returns>True if password matches, false otherwise</returns>
        private bool CheckPassword(string inputPassword, string savedPassword)
        {
            // savedPassword: "hash.salt"
            var parts = savedPassword.Split('.');
            if (parts.Length != 2) return false;
            var hash = parts[0];
            var salt = Convert.FromBase64String(parts[1]);

            // Hash the input password using the same salt and compare
            var inputHash = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: inputPassword,
                    salt: salt,
                    prf: Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
            return hash == inputHash;
        }
    }
}