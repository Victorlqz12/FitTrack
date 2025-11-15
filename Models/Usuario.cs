using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitTrack.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;

        // Relacionamentos 1:N
        public List<Treino>? Treinos { get; set; }
        public List<Evolucao>? Evolucoes { get; set; }
    }
}
