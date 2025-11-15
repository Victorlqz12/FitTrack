using FitTrack.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EvolucoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EvolucoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var evolucoes = _context.Evolucoes.ToList();
            return View(evolucoes);
        }
    }
}
