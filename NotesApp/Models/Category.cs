using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotesApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Eine Kategorie kann mehrere Notizen haben
        public List<Note> Notes { get; set; }
    }
}
