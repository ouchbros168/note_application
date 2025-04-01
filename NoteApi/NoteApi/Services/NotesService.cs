using System.Collections.Generic;
using NoteApi.Models;
using NoteApi.Repositories;
using NoteApi.Repositories.Interfaces;
using NoteApi.Services.Interfaces;

namespace NoteApi.Services
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _repository;


        public NotesService(INotesRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateNote(Note note)
        {
            return await _repository.CreateNote(note);
        }
        
        public async Task<IEnumerable<Note>> GetNotes(int userId, string? title, DateTime? startDate, DateTime? endDate)
        {
            return await _repository.GetNotes(userId, title, startDate, endDate);
        }
        public async Task<Note> GetNoteById(int id, int userId)
        {
            return await _repository.GetNoteById(id, userId);
        }

        public async Task<bool> UpdateNote(Note note)
        {
            return await _repository.UpdateNote(note);
        }

        public async Task<bool> DeleteNote(int id, int userId)
        {
            return await _repository.DeleteNote(id, userId);
        }
    }
}
