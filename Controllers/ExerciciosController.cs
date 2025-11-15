using FitTrack.Data;
using FitTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitTrack.Controllers
{
    public class ExerciciosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exercicios
        public async Task<IActionResult> Index()
        {
            var exercicios = _context.Exercicios.Include(e => e.Treino);
            return View(await exercicios.ToListAsync());
        }

        // GET: Exercicios/Create
        public IActionResult Create()
        {
            ViewBag.Treinos = new SelectList(_context.Treinos, "Id", "NomeTreino");
            return View();
        }

        // POST: Exercicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exercicio exercicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Treinos = new SelectList(_context.Treinos, "Id", "NomeTreino", exercicio.TreinoId);
            return View(exercicio);
        }

        // GET: Exercicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null) return NotFound();

            ViewBag.Treinos = new SelectList(_context.Treinos, "Id", "NomeTreino", exercicio.TreinoId);
            return View(exercicio);
        }

        // POST: Exercicios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exercicio exercicio)
        {
            if (id != exercicio.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Treinos = new SelectList(_context.Treinos, "Id", "NomeTreino", exercicio.TreinoId);
            return View(exercicio);
        }

        // GET: Exercicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios
                                          .Include(e => e.Treino)
                                          .FirstOrDefaultAsync(e => e.Id == id);

            if (exercicio == null) return NotFound();

            return View(exercicio);
        }

        // GET: Exercicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios
                                          .Include(e => e.Treino)
                                          .FirstOrDefaultAsync(e => e.Id == id);

            if (exercicio == null) return NotFound();

            return View(exercicio);
        }

        // POST: Exercicios/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio != null)
            {
                _context.Exercicios.Remove(exercicio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
