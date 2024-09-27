using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Models;
using NotesApp.ViewModels;
using System.Threading.Tasks;

namespace NotesApp.Controllers
{
    public class NotesController : Controller
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        // Index: Zeigt alle Notizen an
        public async Task<IActionResult> Index(int? categoryId)
        {
            var notesQuery = _context.Notes.Include(n => n.Category).AsQueryable();
            if (categoryId.HasValue)
            {
                notesQuery = notesQuery.Where(n => n.CategoryId == categoryId.Value);
            }
            var notes = await notesQuery.OrderByDescending(n => n.Date).ToListAsync();
            return View(notes);
        }


        // Create: Neue Notiz erstellen (GET)
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            var viewModel = new NoteViewModel
            {
                Note = new Note(),
                Categories = new SelectList(categories, "Id", "Name")
            };
            return View(viewModel);
        }

        // Create: Neue Notiz speichern (POST)
        [HttpPost]
        public IActionResult Create(NoteViewModel viewModel)
        {

            var selectedCategory = _context.Categories.FirstOrDefault(c => c.Id == viewModel.Note.CategoryId);
            var maxId = _context.Notes.Max(n => n.Id);


                var newNote = new Note
                {
                    Title = viewModel.Note.Title,
                    Content = viewModel.Note.Content,
                    Date = viewModel.Note.Date.ToUniversalTime(),
                    CategoryId = viewModel.Note.CategoryId,
                    Id = maxId + 1,
                    //Author = User.Identity.Name
                    Author = "Default Author"
                };
                _context.Notes.Add(newNote);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
        }

        // Edit: Bearbeiten einer Notiz (GET)
        public IActionResult Edit(int id)
        {
            var note = _context.Notes.Find(id);

            var categories = _context.Categories.ToList();
            var viewModel = new NoteViewModel
            {
                Note = note,
                Categories = new SelectList(categories, "Id", "Name", note.CategoryId)
            };
            return View(viewModel);
        }


        // Edit: Änderungen speichern (POST)
        [HttpPost]
        public IActionResult Edit(int id, NoteViewModel viewModel)
        {
            // Überprüfen, ob die Notiz mit der übergebenen Id existiert
            var existingNote = _context.Notes.Find(id);


                // Aktualisiere die Werte der existierenden Notiz
                existingNote.Title = viewModel.Note.Title;
                existingNote.Content = viewModel.Note.Content;
                existingNote.Date = viewModel.Note.Date.ToUniversalTime();
                existingNote.CategoryId = viewModel.Note.CategoryId;
                // existingNote.Author könnte ebenfalls aktualisiert werden, wenn erforderlich

                // Speichere die Änderungen in der Datenbank
                _context.Notes.Update(existingNote);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
        }


        // Delete: Notiz löschen (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
