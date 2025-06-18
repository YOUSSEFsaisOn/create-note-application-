using System.ComponentModel.DataAnnotations;

namespace NotesApi.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
