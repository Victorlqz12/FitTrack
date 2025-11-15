using FitTrack.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TreinosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreinosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var treinos = _context.Treinos.Include(t => t.Exercicios).ToList();
            return View(treinos);
        }
    }
}
