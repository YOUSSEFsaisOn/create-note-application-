using NotesApi.Models;
using System;
using System.Collections.Generic;

namespace NotesApi.DTOs
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedAt { get; set; }
        public List<Message> Messages { get; set; }
    }

    public class CreateNoteDto
    {
        public string Title { get; set; }
        public string Content { get; set; }

    }
}