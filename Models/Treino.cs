using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitTrack.Models
{
    public class Treino
    {
        public int Id { get; set; }

        [Required]
        public DateTime Data { get; set; }     

        [Required]
        public string NomeTreino { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; }

        // Um treino tem vários exercícios
        public List<Exercicio>? Exercicios { get; set; }
    }
}
