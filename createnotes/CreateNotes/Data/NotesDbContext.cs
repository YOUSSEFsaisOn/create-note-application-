using Microsoft.EntityFrameworkCore;
using NotesApi.Models;
namespace NotesApi.Data
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure cascade delete for messages when a note is deleted
            modelBuilder.Entity<Note>()
                .HasMany(n => n.Messages)
                .WithOne(n => n.Note)
                .HasForeignKey(m => m.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}