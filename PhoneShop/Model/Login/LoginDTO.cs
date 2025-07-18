using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Model.Login
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
