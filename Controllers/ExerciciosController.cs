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

        public IActionResult Create(int? treinoId)
        {
            var userId = _userManager.GetUserId(User);

            var treinos = _context.Treinos
                .Where(t => t.UserId == userId)
                .ToList();

            // SE NÃO TIVER TREINOS, evitar erro
            if (!treinos.Any())
                treinos.Add(new Treino { Id = 0, NomeTreino = "Nenhum treino disponível" });

            ViewBag.Treinos = treinos;

            var model = new Exercicio();

            if (treinoId.HasValue)
                model.TreinoId = treinoId.Value;

            return View(model);
        }





        // POST: Exercicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exercicio exercicio)
        {
            var user = await _userManager.GetUserAsync(User);
            exercicio.UserId = user.Id;

            // Só por segurança: removemos validação de navegação
            ModelState.Remove("Treino");

            if (!ModelState.IsValid)
            {
                ViewBag.Treinos = new SelectList(
                    _context.Treinos.Where(t => t.UserId == user.Id).ToList(),
                    "Id",
                    "NomeTreino"
                );

                return View(exercicio);
            }

            _context.Exercicios.Add(exercicio);
            await _context.SaveChangesAsync();

            
            return RedirectToAction("Details", "Treinos", new { id = exercicio.TreinoId });
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

            // Validação: O ID da rota DEVE ser o mesmo do ID do formulário
            if (id != exercicio.Id)
            {
                return NotFound();
            }

            // Busca o item original do banco de dados
            var exercicioDb = await _context.Exercicios.FirstOrDefaultAsync(e => e.Id == id);

            if (exercicioDb == null) return NotFound();

            // Segurança: Garante que o usuário só edite seus próprios exercícios
            if (exercicioDb.UserId != user.Id)
            {
                return Unauthorized();
            }

            
            ModelState.Remove("Treino");
            ModelState.Remove("User"); 

            if (ModelState.IsValid)
            {
                try
                {
                    // Mapeia os valores do formulário (exercicio) para o
                    // objeto que veio do banco (exercicioDb)
                    exercicioDb.NomeExercicio = exercicio.NomeExercicio;
                    exercicioDb.Series = exercicio.Series;
                    exercicioDb.Repeticoes = exercicio.Repeticoes;
                    exercicioDb.Carga = exercicio.Carga;
                    exercicioDb.TreinoId = exercicio.TreinoId;

                    
                    
                    await _context.SaveChangesAsync();

                    
                    return RedirectToAction("Details", "Treinos", new { id = exercicio.TreinoId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                    if (!_context.Exercicios.Any(e => e.Id == exercicio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            
            ViewBag.Treinos = new SelectList(
                _context.Treinos.Where(t => t.UserId == user.Id), // A lista de treinos
                "Id",                                              // Valor
                "NomeTreino",                                      // Texto
                exercicio.TreinoId                                 // Item selecionado
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
