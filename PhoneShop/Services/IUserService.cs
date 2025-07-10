using PhoneShop.Model;

namespace PhoneShop.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(UserDTO dto);
        Task<List<UserReadonyDTO>> GetAllUsers();
        Task<UserReadonyDTO> GetUserById(int id);
        Task<bool> UpdateUser(UserDTO dto);
        Task<bool> DeleteUser(int id);
        string CreatePasswordSalt(string password);
    }
}
