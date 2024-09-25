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

        // Details: Details einer Notiz anzeigen
        public async Task<IActionResult> Details(int id)
        {
            var note = await _context.Notes.Include(n => n.Category)
                                            .FirstOrDefaultAsync(n => n.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // Create: Neue Notiz erstellen (GET)
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            var viewModel = new NoteViewModel
            {
                Note = new Note(),
                Categories = new SelectList(categories, "Id", "Name")
            };
            return View(viewModel);
        }

        // Create: Neue Notiz speichern (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            viewModel.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View(viewModel);
        }

        // Edit: Bearbeiten einer Notiz (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            var categories = await _context.Categories.ToListAsync();
            var viewModel = new NoteViewModel
            {
                Note = note,
                Categories = new SelectList(categories, "Id", "Name", note.CategoryId)
            };
            return View(viewModel);
        }

        // Edit: Änderungen speichern (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoteViewModel viewModel)
        {
            if (id != viewModel.Note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(viewModel.Note.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            viewModel.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View(viewModel);
        }

        // Delete: Notiz löschen (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var note = await _context.Notes.Include(n => n.Category)
                                            .FirstOrDefaultAsync(n => n.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // Delete: Notiz löschen (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
