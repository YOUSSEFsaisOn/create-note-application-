using System;

namespace NotesApi.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public string Content { get; set; }
        public string SentAt { get; set; }
    }

    public class CreateMessageDto
    {
        public int NoteId { get; set; }
        public string Content { get; set; }
    }
}