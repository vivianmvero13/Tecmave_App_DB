using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class ResenasModel
    {
        [Key]
        public int id_resena { get; set; } // [pk, increment]
        public int cliente_id { get; set; }
        public int revision_id { get; set; }
        public string comentario { get; set; }
        public float calificacion { get; set; }
    }
}