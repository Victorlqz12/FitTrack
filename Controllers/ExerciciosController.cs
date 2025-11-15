using FitTrack.Data;
using FitTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitTrack.Controllers
{
    [Authorize]
    public class ExerciciosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ExerciciosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // LISTA APENAS EXERCÍCIOS DO USUÁRIO LOGADO
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var exercicios = await _context.Exercicios
                .Include(e => e.Treino)
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return View(exercicios);
        }

        // GET: Exercicios/Create
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);

            // Listar somente treinos do usuário
            ViewBag.Treinos = new SelectList(
                _context.Treinos.Where(t => t.UserId == userId),
                "Id",
                "NomeTreino"
            );

            return View();
        }

        // POST: Exercicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exercicio exercicio)
        {
            var userId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                exercicio.UserId = userId;

                _context.Add(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Treinos = new SelectList(
                _context.Treinos.Where(t => t.UserId == userId),
                "Id",
                "NomeTreino",
                exercicio.TreinoId
            );

            return View(exercicio);
        }

        // GET: Exercicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null) return NotFound();

            // Impede editar exercício de outro usuário
            if (exercicio.UserId != _userManager.GetUserId(User))
                return Forbid();

            var userId = _userManager.GetUserId(User);

            ViewBag.Treinos = new SelectList(
                _context.Treinos.Where(t => t.UserId == userId),
                "Id",
                "NomeTreino",
                exercicio.TreinoId
            );

            return View(exercicio);
        }

        // POST: Exercicios/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exercicio exercicio)
        {
            if (id != exercicio.Id) return NotFound();

            var userId = _userManager.GetUserId(User);

            if (exercicio.UserId != userId)
                return Forbid();

            if (ModelState.IsValid)
            {
                _context.Update(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Treinos = new SelectList(
                _context.Treinos.Where(t => t.UserId == userId),
                "Id",
                "NomeTreino",
                exercicio.TreinoId
            );

            return View(exercicio);
        }

        // GET: Exercicios/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios
                .Include(e => e.Treino)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercicio == null) return NotFound();

            if (exercicio.UserId != _userManager.GetUserId(User))
                return Forbid();

            return View(exercicio);
        }

        // GET: Exercicios/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios
                .Include(e => e.Treino)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercicio == null) return NotFound();

            if (exercicio.UserId != _userManager.GetUserId(User))
                return Forbid();

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
                if (exercicio.UserId != _userManager.GetUserId(User))
                    return Forbid();

                _context.Exercicios.Remove(exercicio);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
