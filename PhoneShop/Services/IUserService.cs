using PhoneShop.Model;

namespace PhoneShop.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(RegisterDTO dto);
        Task<List<UserReadonyDTO>> GetAllUsers();
        Task<UserReadonyDTO> GetUserById(int id);
        Task<UserDTO> GetUserByUsername(string username);
        Task<bool> UpdateUser(UserUpdateDTO dto);
        Task<bool> DeleteUser(int id);
        string CreatePasswordSalt(string password);
    }
}
