using Microsoft.AspNetCore.Mvc.Rendering;
using NotesApp.Models;

namespace NotesApp.ViewModels
{
    public class NoteViewModel
    {
        public Note Note { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
