using NoteApi.Models;

namespace NoteApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<int> CreateUser(User user);
    }
}
