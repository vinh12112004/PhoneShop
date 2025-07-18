using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model.User;
using System.Security.Cryptography;

namespace PhoneShop.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IPhoneShopRepository<User> _userRepository;
        public UserService(IPhoneShopRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public string CreatePasswordSalt(string password)
        {
            //create salt
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            //create password hash
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return $"{hash}.{Convert.ToBase64String(salt)}";
        }
        // create user
        public async Task<bool> CreateUser(RegisterDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, $"the argument {nameof(dto)} is null");
            // call GetAsync to check if the user already exists
            var existingUser = await _userRepository.GetAsync(x => x.Username.Equals(dto.Username));
            // if user exists, throw an exception
            if (existingUser != null)
            {
                throw new ArgumentException($"User with username {dto.Username} already exists.");
            }
            // map dto to User entity
            var user = _mapper.Map<User>(dto);
            // create password salt
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.Password = CreatePasswordSalt(dto.Password);
            }
            await _userRepository.CreateAsync(user);
            return true;

        }
        // get all users
        public async Task<List<UserReadonyDTO>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserReadonyDTO>>(users);
        }
        // get user by id
        public async Task<UserReadonyDTO> GetUserById(int id)
        {
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            return _mapper.Map<UserReadonyDTO>(user);
        }
        // get user by username
        public async Task<UserDTO> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetAsync(u => u.Username == username, q => q.Include(x => x.Role));
            if (user == null)
            {
                throw new KeyNotFoundException($"User with username {username} not found.");
            }
            return _mapper.Map<UserDTO>(user);
        }
        // update user
        public async Task<bool> UpdateUser(UserUpdateDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, $"the argument {nameof(dto)} is null");
            // call GetAsync to check if the user exists
            var user = await _userRepository.GetAsync(x => x.Id == dto.Id, true);
            // if user does not exist, throw an exception
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {dto.Id} not found.");
            }
            _mapper.Map(dto, user);
            // if password is provided, create password salt
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.Password = CreatePasswordSalt(dto.Password);
            }
            await _userRepository.UpdateAsync(user);
            return true;
        }
        // delete user
        public async Task<bool> DeleteUser(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }
            var user = await _userRepository.GetAsync(x => x.Id == id, true);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            await _userRepository.DeleteAsync(user);
            return true;
        }
    }
}
