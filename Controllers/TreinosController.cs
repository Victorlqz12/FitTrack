using FitTrack.Data;
using FitTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FitTrack.Controllers
{
    [Authorize]
    public class TreinosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TreinosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Treinos
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var treinos = await _context.Treinos
                .Include(t => t.Exercicios)
                .Where(t => t.UserId == user.Id)
                .OrderByDescending(t => t.Data)
                .ToListAsync();

            return View(treinos);
        }


        // GET: Treinos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Treinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Treino treino)
        {
            var user = await _userManager.GetUserAsync(User);
            treino.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(treino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(treino);
        }

        // GET: Treinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var treino = await _context.Treinos.FindAsync(id);

            if (treino == null) return NotFound();

            if (treino.UserId != user.Id)
                return Unauthorized();

            return View(treino);
        }


        // POST: Treinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Treino treino)
        {
            var user = await _userManager.GetUserAsync(User);

            var treinoDb = await _context.Treinos.FindAsync(id);

            if (treinoDb == null) return NotFound();
            if (treinoDb.UserId != user.Id) return Unauthorized();

            treinoDb.Data = treino.Data;
            treinoDb.NomeTreino = treino.NomeTreino;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Treinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null) return NotFound();

            if (treino.UserId != _userManager.GetUserId(User))
                return Forbid();

            return View(treino);
        }

        // GET: Treinos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino == null) return NotFound();

            if (treino.UserId != _userManager.GetUserId(User))
                return Forbid();

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
                if (treino.UserId != _userManager.GetUserId(User))
                    return Forbid();

                _context.Treinos.Remove(treino);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
