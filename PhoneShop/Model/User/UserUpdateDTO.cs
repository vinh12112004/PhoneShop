﻿namespace PhoneShop.Model.User
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string AddressCompany { get; set; }
        public string AddressHome { get; set; }
    }
}
