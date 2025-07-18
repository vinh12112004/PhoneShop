using PhoneShop.Model.User;

namespace PhoneShop.Services.UserService
{
    public interface IUserService
    {
        // Create a new user
        Task<bool> CreateUser(RegisterDTO dto);
        // get all users
        Task<List<UserReadonyDTO>> GetAllUsers();
        // get user by id
        Task<UserReadonyDTO> GetUserById(int id);
        // get user by username
        Task<UserDTO> GetUserByUsername(string username);
        // update user
        Task<bool> UpdateUser(UserUpdateDTO dto);
        // delete user
        Task<bool> DeleteUser(int id);
        //create password salt
        string CreatePasswordSalt(string password);
    }
}
