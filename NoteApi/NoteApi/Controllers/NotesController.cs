using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NoteApi.Models;
using NoteApi.Services;
using NoteApi.Services.Interfaces;

namespace NoteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints
    public class NotesController : ControllerBase
    {
       
        private readonly INotesService _service;

        public NotesController(INotesService service)
        {
            _service = service;
        }
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Note note)
        {
            note.UserId = GetCurrentUserId();
            var noteId = await _service.CreateNote(note);
            return CreatedAtAction(nameof(GetById), new { id = noteId }, note);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? title,[FromQuery] DateTime? startDate,[FromQuery] DateTime? endDate)
        {
            int userId =  GetCurrentUserId();
            var notes = await _service.GetNotes(userId, title, startDate, endDate);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            int userId =  GetCurrentUserId();
            var note = await _service.GetNoteById(id, userId);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Note note)
        {
            if (id != note.Id) return BadRequest();

            note.UserId =  GetCurrentUserId();
            var success = await _service.UpdateNote(note);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = GetCurrentUserId();
            var success = await _service.DeleteNote(id, userId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
