using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class RevisionModel
    {
        [Key]
        public int id_revision { get; set; } // [pk, increment]
        public string vehiculo_id { get; set; }
        public DateTime fecha_ingreso { get; set; }
        public int id_servicio { get; set; }
        public int id_estado { get; set; }
        public DateTime fecha_estimada_entrega { get; set; }
        public DateTime fecha_entrega_final { get; set; }

    }
}