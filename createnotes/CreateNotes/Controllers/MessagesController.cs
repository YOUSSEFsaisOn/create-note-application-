using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Models;
using NotesApi.DTOs;

namespace NotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly NotesDbContext _context;

        public MessagesController(NotesDbContext context)
        {
            _context = context;
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<MessageDto>> SendMessage(CreateMessageDto messageDto)
        {
            var note = await _context.Notes.FindAsync(messageDto.NoteId);
            if (note == null)
            {
                return BadRequest("Note not found");
            }

            var message = new Message
            {
                NoteId = messageDto.NoteId,
                Content = messageDto.Content,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMessage),
                new { id = message.Id },
                new MessageDto
                {
                    Id = message.Id,
                    NoteId = message.NoteId,
                    Content = message.Content,
                    SentAt = message.SentAt.ToString("o")
                });
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDto>> GetMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return new MessageDto
            {
                Id = message.Id,
                NoteId = message.NoteId,
                Content = message.Content,
                SentAt = message.SentAt.ToString("o")
            };
        }

        // GET: api/Messages/note/5
        [HttpGet("note/{noteId}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesByNoteId(int noteId)
        {
            var messages = await _context.Messages
                .Where(m => m.NoteId == noteId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return messages.Select(m => new MessageDto
            {
                Id = m.Id,
                NoteId = m.NoteId,
                Content = m.Content,
                SentAt = m.SentAt.ToString("o")
            }).ToList();
        }
    }
}
