using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace NotesApi.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public int NoteId { get; set; }

        [JsonIgnore]
        public Note Note { get; set; }
    }
}