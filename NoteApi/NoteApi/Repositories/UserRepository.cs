using Dapper;
using Microsoft.Data.SqlClient;
using NoteApi.Models;
using NoteApi.Repositories.Interfaces;

namespace NoteApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT * FROM Users WHERE Username = @Username";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<int> CreateUser(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash); SELECT SCOPE_IDENTITY();";
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }
    }
}
