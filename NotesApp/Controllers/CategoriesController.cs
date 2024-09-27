using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Data;
using NotesApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // Index: Zeigt alle Kategorien an
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }


        // Create: Neue Kategorie erstellen (GET)
        public IActionResult Create()
        {
            return View(nameof(Create));
        }


        // Create: Neue Kategorie speichern (POST)
        [HttpPost]
        public IActionResult Create(Category category)
        {
            var maxId = _context.Categories.Max(x => x.Id);
            category.Id = maxId + 1;
            _context.Add(category);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GetCategory By ID 

        public Category? GetCategoryById(int id)
        {
            var categoryToUpdate = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (categoryToUpdate != null)
            {
                return new Category
                {
                    Id = categoryToUpdate.Id,
                    Name = categoryToUpdate.Name,
                };
            }
            return null;
        }


        // Edit: Bearbeiten einer Kategorie (GET)
        public IActionResult Edit(int id)
        {
            var category = GetCategoryById(id);
            return View(category);
        }

        // Edit: Änderungen speichern (POST)
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Name != null)
            {
                _context.Update(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Delete: Kategorie löschen (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
