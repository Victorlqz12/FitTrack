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
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var exercicios = await _context.Exercicios
                .Include(e => e.Treino)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Id)
                .ToListAsync();

            return View(exercicios);
        }


        // GET: Exercicios/Create
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);

            var treinos = _context.Treinos
                .Where(t => t.UserId == userId)
                .ToList();

            if (!treinos.Any())
            {
                TempData["Erro"] = "Você precisa criar um TREINO antes de cadastrar exercícios.";
                return RedirectToAction("Index");
            }

            ViewBag.Treinos = new SelectList(treinos, "Id", "NomeTreino");

            return View();
        }



        // POST: Exercicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exercicio exercicio)
        {
            var user = await _userManager.GetUserAsync(User);
            exercicio.UserId = user.Id;

            if (ModelState.IsValid)
            {
                _context.Add(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Treinos = new SelectList(
                 _context.Treinos.Where(t => t.UserId == user.Id).ToList(),
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

            var user = await _userManager.GetUserAsync(User);
            var exercicio = await _context.Exercicios.FirstOrDefaultAsync(e => e.Id == id);

            if (exercicio == null) return NotFound();

            // Não permitir editar exercício de outra pessoa
            if (exercicio.UserId != user.Id)
                return Unauthorized();

            ViewBag.Treinos = new SelectList(
                _context.Treinos.Where(t => t.UserId == user.Id),
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
            var user = await _userManager.GetUserAsync(User);

            var exercicioDb = await _context.Exercicios.FirstOrDefaultAsync(e => e.Id == id);

            if (exercicioDb == null) return NotFound();

            // Não permitir editar exercício de outra pessoa
            if (exercicioDb.UserId != user.Id)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                exercicioDb.NomeExercicio = exercicio.NomeExercicio;
                exercicioDb.Series = exercicio.Series;
                exercicioDb.Repeticoes = exercicio.Repeticoes;
                exercicioDb.Carga = exercicio.Carga;
                exercicioDb.TreinoId = exercicio.TreinoId;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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

            var user = await _userManager.GetUserAsync(User);

            var exercicio = await _context.Exercicios
                .Include(e => e.Treino)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercicio == null) return NotFound();

            if (exercicio.UserId != user.Id)
                return Unauthorized();

            return View(exercicio);
        }


        // POST: Exercicios/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var exercicio = await _context.Exercicios.FirstOrDefaultAsync(e => e.Id == id);

            if (exercicio == null) return NotFound();

            if (exercicio.UserId != user.Id)
                return Unauthorized();

            _context.Exercicios.Remove(exercicio);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
