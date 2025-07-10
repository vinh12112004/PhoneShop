namespace PhoneShop.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string AddressCopany { get; set; }
        public string AddressHome { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
