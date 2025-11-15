using FitTrack.Data;
using FitTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    // LISTAR APENAS TREINOS DO USUÁRIO
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var treinos = await _context.Treinos
            .Include(t => t.Exercicios)
            .Where(t => t.UserId == user.Id)
            .OrderByDescending(t => t.Data)
            .ToListAsync();

        return View(treinos);
    }

    // TELA CRIAÇÃO
    public IActionResult Create()
    {
        return View();
    }

    // CRIAR TREINO
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Treino treino)
    {
        ModelState.Remove("UserId"); // evitar erro de validação

        var user = await _userManager.GetUserAsync(User);
        treino.UserId = user.Id;

        if (!ModelState.IsValid)
        {
            return View(treino);
        }

        _context.Add(treino);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }



    // EDITAR (GET)
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var treino = await _context.Treinos.FindAsync(id);
        var user = await _userManager.GetUserAsync(User);

        if (treino == null) return NotFound();
        if (treino.UserId != user.Id) return Unauthorized();

        return View(treino);
    }

    // EDITAR (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Treino treino)
    {
        var treinoDb = await _context.Treinos.FindAsync(id);
        var user = await _userManager.GetUserAsync(User);

        if (treinoDb == null) return NotFound();
        if (treinoDb.UserId != user.Id) return Unauthorized();

        treinoDb.NomeTreino = treino.NomeTreino;
        treinoDb.Data = treino.Data;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // DETALHES
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);

        var treino = await _context.Treinos
            .Include(t => t.Exercicios)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

        if (treino == null) return NotFound();

        return View(treino);
    }

    // DELETE (GET)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);

        var treino = await _context.Treinos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

        if (treino == null) return Unauthorized();

        return View(treino);
    }

    // DELETE (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var treino = await _context.Treinos.FindAsync(id);
        var user = await _userManager.GetUserAsync(User);

        if (treino == null || treino.UserId != user.Id)
            return Unauthorized();

        _context.Treinos.Remove(treino);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
