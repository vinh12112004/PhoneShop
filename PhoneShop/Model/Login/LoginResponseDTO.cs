using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Model.Login
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
