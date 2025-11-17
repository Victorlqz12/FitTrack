using FitTrack.Data;
using FitTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Controllers
{
    [Authorize]
    public class EvolucoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EvolucoesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Evolucoes - lista apenas do usuário logado
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var evolucoes = await _context.Evolucoes
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.DataRegistro)
                .ToListAsync();

            return View(evolucoes);
        }

     
        // GET: Evolucoes/Create
        public IActionResult Create()
        {
            // 1. Crie o novo objeto
            var evolucao = new Evolucao
            {
                // 2. Defina o valor padrão AQUI, no Controller
                DataRegistro = DateTime.Now
            };

            // 3. Envie o objeto (que não é mais nulo) para a View
            return View(evolucao);
        }


        // POST: Evolucoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evolucao evolucao)
        {
            
            ModelState.Remove("UserId");
            ModelState.Remove("User"); 

            if (ModelState.IsValid)
            {
                
                evolucao.UserId = _userManager.GetUserId(User);

                _context.Add(evolucao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            
            return View(evolucao);
        }

        // GET: Evolucoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var evolucao = await _context.Evolucoes.FindAsync(id);
            if (evolucao == null) return NotFound();

            if (evolucao.UserId != _userManager.GetUserId(User))
                return Forbid();

            return View(evolucao);
        }

        // POST: Evolucoes/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evolucao evolucao)
        {
            if (id != evolucao.Id)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            // 1. Busca o item ORIGINAL do banco
            var evolucaoDb = await _context.Evolucoes.FirstOrDefaultAsync(e => e.Id == id);

            // 2. VERIFICA O DONO
            if (evolucaoDb == null || evolucaoDb.UserId != userId)
            {
                return Forbid();
            }

            // Remove validação de propriedades de navegação (se houver)
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    
                    evolucaoDb.DataRegistro = evolucao.DataRegistro;
                    evolucaoDb.Peso = evolucao.Peso;

                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Evolucoes.Any(e => e.Id == evolucao.Id))
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


            return View(evolucao);
        }

        // GET: Evolucoes/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var evolucao = await _context.Evolucoes
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evolucao == null) return NotFound();

            if (evolucao.UserId != _userManager.GetUserId(User))
                return Forbid();

            return View(evolucao);
        }

        // GET: Evolucoes/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var evolucao = await _context.Evolucoes
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evolucao == null) return NotFound();

            if (evolucao.UserId != _userManager.GetUserId(User))
                return Forbid();

            return View(evolucao);
        }

        // POST: Evolucoes/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evolucao = await _context.Evolucoes.FindAsync(id);

            if (evolucao != null)
            {
                if (evolucao.UserId != _userManager.GetUserId(User))
                    return Forbid();

                _context.Evolucoes.Remove(evolucao);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}