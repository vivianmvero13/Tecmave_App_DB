using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class ServiciosRevisionModel
    {
        [Key]
        public int id_servicio_revision { get; set; } // [pk, increment]
        public int revision_id { get; set; }
        public int servicio_id { get; set; }
        public decimal costo_final { get; set; }
    }
}