using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Model
{
    public class LoginDTO
    {
        [Required]
        public String Username { get; set; }
        [Required]
        public String Password { get; set; }
    }
}
