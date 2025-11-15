using FitTrack.Data;
using FitTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Controllers
{
    public class TreinosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreinosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Treinos
        public async Task<IActionResult> Index()
        {
            var treinos = _context.Treinos.Include(t => t.Usuario);
            return View(await treinos.ToListAsync());
        }

        // GET: Treinos/Create
        public IActionResult Create()
        {
            ViewBag.Usuarios = _context.Usuarios.ToList();
            return View();
        }

        // POST: Treinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Treino treino)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Usuarios = _context.Usuarios.ToList();
            return View(treino);
        }

        // GET: Treinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var treino = await _context.Treinos.FindAsync(id);
            if (treino == null) return NotFound();

            ViewBag.Usuarios = _context.Usuarios.ToList();
            return View(treino);
        }

        // POST: Treinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Treino treino)
        {
            if (id != treino.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(treino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Usuarios = _context.Usuarios.ToList();
            return View(treino);
        }

        // GET: Treinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                                       .Include(t => t.Usuario)
                                       .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null) return NotFound();

            return View(treino);
        }

        // GET: Treinos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                                       .Include(t => t.Usuario)
                                       .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null) return NotFound();

            return View(treino);
        }

        // POST: Treinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treino = await _context.Treinos.FindAsync(id);

            if (treino != null)
            {
                _context.Treinos.Remove(treino);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
