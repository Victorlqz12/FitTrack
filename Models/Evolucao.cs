using System;
using System.ComponentModel.DataAnnotations;

namespace FitTrack.Models
{
    public class Evolucao
    {
        public int Id { get; set; }

        [Required]
        public DateTime DataRegistro { get; set; }

        [Required]
        public string UserId { get; set; }


        [Required]
        public double Peso { get; set; }

       
    }
}
