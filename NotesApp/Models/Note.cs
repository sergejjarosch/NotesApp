using System.ComponentModel.DataAnnotations;

namespace NotesApp.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string? Author { get; set; }

    
        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }
    }


}
