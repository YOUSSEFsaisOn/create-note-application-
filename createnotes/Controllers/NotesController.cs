using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Models;
using NotesApi.DTOs;

namespace NotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesDbContext _context;

        public NotesController(NotesDbContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotes()
        {
            var notes = await _context.Notes
                .AsNoTracking()
                .Include(p => p.Messages)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt.ToString("o"),
                    Messages = n.Messages.ToList(),

                })
                .ToListAsync();

            return Ok(notes);
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetNote(int id)
        {
            var note = await _context.Notes
                .AsNoTracking()
                .Include(p => p.Messages)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt.ToString("o")
            });
        }

        // GET: api/Notes/search?query=keyword
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> SearchNotes(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return await GetNotes();
            }

            var notes = await _context.Notes
                .AsNoTracking()
                .Where(n => n.Title.ToLower().Contains(query.ToLower()) || n.Content.ToLower().Contains(query.ToLower()))
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt.ToString("o")
                })
                .ToListAsync();

            return Ok(notes);
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<ActionResult<NoteDto>> CreateNote(CreateNoteDto noteDto)
        {
            if (noteDto == null || string.IsNullOrWhiteSpace(noteDto.Title) || string.IsNullOrWhiteSpace(noteDto.Content))
            {
                return BadRequest("Title and Content are required.");
            }

            var note = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            var createdNote = new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt.ToString("o")
            };

            return CreatedAtAction(nameof(GetNote), new { id = note.Id }, createdNote);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditNote(int id, [FromBody] NoteDto noteDto)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound("Note not found");
            }

            note.Title = noteDto.Title;
            note.Content = noteDto.Content;
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
