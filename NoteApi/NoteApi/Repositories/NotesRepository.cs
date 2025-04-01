using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using NoteApi.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using NoteApi.Repositories.Interfaces;

namespace NoteApi.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly string _connectionString;

        public NotesRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateNote(Note note)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO Notes (UserId, Title, Content, CreatedAt) VALUES (@UserId, @Title, @Content, GETDATE()); SELECT SCOPE_IDENTITY();";
            return await connection.ExecuteScalarAsync<int>(sql, note);
        }

        public async Task<IEnumerable<Note>> GetNotes(int userId, string? title = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = "SELECT * FROM Notes WHERE UserId = @UserId";

            if (!string.IsNullOrEmpty(title))
                sql += " AND Title LIKE @Title";
            if (startDate.HasValue)
                sql += " AND CreatedAt >= @StartDate";
            if (endDate.HasValue)
                sql += " AND CreatedAt <= @EndDate";

            return await connection.QueryAsync<Note>(sql, new
            {
                UserId = userId,
                Title = "%" + title + "%",
                StartDate = startDate,
                EndDate = endDate
            });
        }

        public async Task<Note> GetNoteById(int id, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Notes WHERE Id = @Id AND UserId = @UserId";
            return await connection.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
        }

        public async Task<bool> UpdateNote(Note note)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Notes SET Title = @Title, Content = @Content, UpdatedAt = GETDATE() WHERE Id = @Id AND UserId = @UserId";
            var rowsAffected = await connection.ExecuteAsync(sql, note);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteNote(int id, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
            return rowsAffected > 0;
        }
    }
}
