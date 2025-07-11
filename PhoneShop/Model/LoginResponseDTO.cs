using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Model
{
    public class LoginResponseDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Token { get; set; }
        public string RoleName { get; set; }
    }
}
