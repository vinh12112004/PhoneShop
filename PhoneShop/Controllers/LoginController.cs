using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhoneShop.Model;
using PhoneShop.Services;
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
        public LoginController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("pls provide password and username");
            }
            var user = await _userService.GetUserByUsername(model.Username);
            LoginResponseDTO reponse = new()
            {
                Username = model.Username,
                RoleName = user.RoleName
            };
            if (!CheckPassword(model.Password, user.Password))
            {
                return BadRequest("Invalid username or password");
            }

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
            
            return Ok(reponse);
        }
        private bool CheckPassword(string inputPassword, string savedPassword)
        {
            // savedPassword: "hash.salt"
            var parts = savedPassword.Split('.');
            if (parts.Length != 2) return false;
            var hash = parts[0];
            var salt = Convert.FromBase64String(parts[1]);

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
