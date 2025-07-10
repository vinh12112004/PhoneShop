using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PhoneShop.Data;
using PhoneShop.Data.Repository;
using PhoneShop.Model;
using System.Security.Cryptography;

namespace PhoneShop.Services
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
        public async Task<bool> CreateUser(UserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, $"the argument {nameof(dto)} is null");
            var existingUser = await _userRepository.GetAsync(x => x.Username.Equals(dto.Username));
            if (existingUser != null)
            {
                throw new ArgumentException($"User with username {dto.Username} already exists.");
            }
            var user = _mapper.Map<User>(dto);
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.Password = CreatePasswordSalt(dto.Password);
            }
            await _userRepository.CreateAsync(user);
            return true;

        }
        public async Task<List<UserReadonyDTO>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserReadonyDTO>>(users);
        }
        public async Task<UserReadonyDTO> GetUserById(int id)
        {
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            return _mapper.Map<UserReadonyDTO>(user);
        }
        public async Task<bool> UpdateUser(UserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, $"the argument {nameof(dto)} is null");
            var user = await _userRepository.GetAsync(x => x.Id == dto.Id, true);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {dto.Id} not found.");
            }
            var userToUpdate = _mapper.Map<User>(dto);
            if (!string.IsNullOrEmpty(dto.Password))
            {
                userToUpdate.Password = CreatePasswordSalt(dto.Password);
            }
            await _userRepository.UpdateAsync(userToUpdate);
            return true;
        }
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
