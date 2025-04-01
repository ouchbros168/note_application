using NoteApi.Models;

namespace NoteApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<int?> Register(string username, string password);
        Task<User?> Authenticate(string username, string password);
    }
}
