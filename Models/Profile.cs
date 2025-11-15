using System;
using System.ComponentModel.DataAnnotations;

namespace FitTrack.Models
{
    public class Profile
    {
        public int Id { get; set; }

        // FK para IdentityUser
        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(60)]
        public string Nome { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Display(Name = "Altura (cm)")]
        public int? Altura { get; set; }

        [Display(Name = "Peso Inicial (kg)")]
        public double? PesoInicial { get; set; }

        public string? Sexo { get; set; }
    }
}
