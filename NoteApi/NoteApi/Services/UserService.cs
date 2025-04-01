using System.Security.Cryptography;
using NoteApi.Models;
using NoteApi.Repositories;
using System.Text;
using NoteApi.Services.Interfaces;
using NoteApi.Repositories.Interfaces;

namespace NoteApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int?> Register(string username, string password)
        {
            var existingUser = await _userRepository.GetUserByUsername(username);
            if (existingUser != null) return null; // User already exists

            string hashedPassword = HashPassword(password);
            var user = new User { Username = username, PasswordHash = hashedPassword };
            return await _userRepository.CreateUser(user);
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null; // Invalid credentials

            return user;
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string hashedEnteredPassword = HashPassword(enteredPassword);
            return hashedEnteredPassword == storedHash;
        }
    }
}
