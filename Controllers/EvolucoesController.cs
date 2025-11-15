using FitTrack.Data;
using FitTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitTrack.Controllers
{
    public class EvolucoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EvolucoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Evolucoes
        public async Task<IActionResult> Index()
        {
            var evolucoes = _context.Evolucoes.Include(e => e.Usuario);
            return View(await evolucoes.ToListAsync());
        }

        // GET: Evolucoes/Create
        public IActionResult Create()
        {
            ViewBag.Usuarios = new SelectList(_context.Usuarios, "Id", "Nome");
            return View();
        }

        // POST: Evolucoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evolucao evolucao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evolucao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Usuarios = new SelectList(_context.Usuarios, "Id", "Nome", evolucao.UsuarioId);
            return View(evolucao);
        }

        // GET: Evolucoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var evolucao = await _context.Evolucoes.FindAsync(id);
            if (evolucao == null) return NotFound();

            ViewBag.Usuarios = new SelectList(_context.Usuarios, "Id", "Nome", evolucao.UsuarioId);
            return View(evolucao);
        }

        // POST: Evolucoes/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evolucao evolucao)
        {
            if (id != evolucao.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(evolucao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Usuarios = new SelectList(_context.Usuarios, "Id", "Nome", evolucao.UsuarioId);
            return View(evolucao);
        }

        // GET: Evolucoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var evolucao = await _context.Evolucoes
                                         .Include(e => e.Usuario)
                                         .FirstOrDefaultAsync(e => e.Id == id);

            if (evolucao == null) return NotFound();

            return View(evolucao);
        }

        // GET: Evolucoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var evolucao = await _context.Evolucoes
                                         .Include(e => e.Usuario)
                                         .FirstOrDefaultAsync(e => e.Id == id);

            if (evolucao == null) return NotFound();

            return View(evolucao);
        }

        // POST: Evolucoes/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evolucao = await _context.Evolucoes.FindAsync(id);

            if (evolucao != null)
            {
                _context.Evolucoes.Remove(evolucao);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
