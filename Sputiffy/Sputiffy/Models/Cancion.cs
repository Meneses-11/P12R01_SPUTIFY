using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sputiffy.Models
{
    [Table("CancionesOffline")]
    public class Cancion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(200)]
        public string Artista { get; set; }

        [StringLength(200)]
        public string Album { get; set; }

        [Required]
        [StringLength(500)]
        public string RutaArchivo { get; set; }

        // Duración en segundos (puede ser null si no se conoce)
        public int? Duracion { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}