using FitTrack.Data;
using FitTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class TreinoExerciciosController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TreinoExerciciosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // TELA PARA ADICIONAR EXERCÍCIO AO TREINO
    public async Task<IActionResult> Create(int treinoId)
    {
        var user = await _userManager.GetUserAsync(User);

        ViewBag.Exercicios = await _context.Exercicios
            .Where(e => e.UserId == user.Id)
            .ToListAsync();

        return View(new TreinoExercicio { TreinoId = treinoId });
    }


    [HttpPost]
    public async Task<IActionResult> Create(TreinoExercicio model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _context.TreinoExercicios.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Treinos", new { id = model.TreinoId });
    }


    // EDITAR
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _context.TreinoExercicios
            .Include(t => t.Exercicio)
            .Include(t => t.Treino)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, TreinoExercicio form)
    {
        var model = await _context.TreinoExercicios.FindAsync(id);

        if (model == null) return NotFound();

        model.Series = form.Series;
        model.Repeticoes = form.Repeticoes;
        model.Carga = form.Carga;

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Treinos", new { id = model.TreinoId });
    }


    // REMOVER EXERCÍCIO DO TREINO
    public async Task<IActionResult> Delete(int id)
    {
        var model = await _context.TreinoExercicios
            .Include(t => t.Exercicio)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (model == null) return NotFound();

        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var registro = await _context.TreinoExercicios.FindAsync(id);
        if (registro == null) return NotFound();

        int treinoId = registro.TreinoId;

        _context.TreinoExercicios.Remove(registro);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Treinos", new { id = treinoId });
    }
}
