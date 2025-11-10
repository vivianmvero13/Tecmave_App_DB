using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class ResenasModel
    {
        [Key]
        public int id_resena { get; set; } // PK (identity)
        [Required]
        public string cliente_id { get; set; } = string.Empty; // <-- string para Identity
        [Required]
        public int servicio_id { get; set; }
        [Required, MaxLength(2000)]
        public string comentario { get; set; } = string.Empty;
        [Range(1, 5)]
        public float calificacion { get; set; }
        public DateTime fecha { get; set; } = DateTime.UtcNow;
    }
}
