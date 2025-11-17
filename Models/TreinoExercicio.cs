using System.ComponentModel.DataAnnotations;

namespace FitTrack.Models
{
    public class TreinoExercicio
    {
        public int Id { get; set; }

        [Required]
        public int TreinoId { get; set; }
        public Treino? Treino { get; set; }

        [Required]
        public int ExercicioId { get; set; }
        public Exercicio? Exercicio { get; set; }

        public int Series { get; set; }
        public int Repeticoes { get; set; }
        public double Carga { get; set; }
    }
}
