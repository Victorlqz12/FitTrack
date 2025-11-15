using System.ComponentModel.DataAnnotations;

namespace FitTrack.Models
{
    public class Exercicio
    {
        public int Id { get; set; }

        [Required]
        public string NomeExercicio { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; }

        public int Series { get; set; }
        public int Repeticoes { get; set; }
        public double Carga { get; set; }

        // FK
        public int TreinoId { get; set; }
        public Treino? Treino { get; set; }
    }
}

