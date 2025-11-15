using FitTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FitTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<Treino> Treinos => Set<Treino>();
        public DbSet<Exercicio> Exercicios => Set<Exercicio>();
        public DbSet<Evolucao> Evolucoes => Set<Evolucao>();
        public DbSet<Profile> Profiles { get; set; }


    }
}
