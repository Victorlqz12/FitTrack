using FitTrack.Data;
using FitTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // VISUALIZAÇÃO DO PERFIL
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                profile = new Profile
                {
                    UserId = user.Id,
                    Nome = ""
                };

                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();
            }

            return View(profile);
        }

        // TELA DE EDIÇÃO
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null) return NotFound();

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Profile model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            // Busca o perfil correto
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
                return NotFound();

            // Atualiza os campos
            profile.Nome = model.Nome;
            profile.Altura = model.Altura;
            profile.DataNascimento = model.DataNascimento;
            profile.PesoInicial = model.PesoInicial;
            profile.Sexo = model.Sexo;

            // Salva no banco
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }








    }
}
