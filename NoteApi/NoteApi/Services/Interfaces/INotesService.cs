using NoteApi.Models;

namespace NoteApi.Services.Interfaces
{
    public interface INotesService
    {
        Task<int> CreateNote(Note note);
        Task<IEnumerable<Note>> GetNotes(int userId, string? title = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<Note> GetNoteById(int id, int userId);
        Task<bool> UpdateNote(Note note);
        Task<bool> DeleteNote(int id, int userId);
    }
}
