using System.ComponentModel.DataAnnotations;

namespace FitTrack.Models
{
    public class Exercicio
    {
        public int Id { get; set; }

        [Required]
        public string NomeExercicio { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty; 
        public int Series { get; set; }
        public int Repeticoes { get; set; }
        public double Carga { get; set; }

        // FK
        [Required]
        public int TreinoId { get; set; }
        public Treino? Treino { get; set; }
    }
}
