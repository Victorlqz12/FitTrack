using FitTrack.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var totalTreinos = await _context.Treinos
                .Where(t => t.UserId == userId)
                .CountAsync();

            var totalExercicios = await _context.Exercicios
                .Where(e => e.UserId == userId)
                .CountAsync();

            var evolucoes = await _context.Evolucoes
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.DataRegistro)
                .ToListAsync();

            double pesoAtual = evolucoes.LastOrDefault()?.Peso ?? 0;
            double pesoInicial = evolucoes.FirstOrDefault()?.Peso ?? pesoAtual;
            double diferencaPeso = pesoAtual - pesoInicial;

            ViewBag.TotalTreinos = totalTreinos;
            ViewBag.TotalExercicios = totalExercicios;
            ViewBag.PesoAtual = pesoAtual;
            ViewBag.DiferencaPeso = diferencaPeso;
            ViewBag.Datas = evolucoes.Select(x => x.DataRegistro.ToShortDateString()).ToList();
            ViewBag.Pesos = evolucoes.Select(x => x.Peso).ToList();

            return View();
        }
    }
}
