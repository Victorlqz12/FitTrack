using FitTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FitTrack.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Treino> Treinos => Set<Treino>();
        public DbSet<Exercicio> Exercicios => Set<Exercicio>();
        public DbSet<Evolucao> Evolucoes => Set<Evolucao>();
    }
}
