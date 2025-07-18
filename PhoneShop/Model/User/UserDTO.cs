using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PhoneShop.Model.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string AddressCompany { get; set; }
        public string AddressHome { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
