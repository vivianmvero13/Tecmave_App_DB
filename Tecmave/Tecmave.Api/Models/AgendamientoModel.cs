using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class AgendamientoModel
    {
        [Key]
        public int id_agendamiento { get; set; } // [pk, increment]
        public int cliente_id { get; set; }// [fk, not null]
        public int vehiculo_id { get; set; } // [fk, not null]
        public DateOnly fecha_agregada { get; set; }
        public int id_estado { get; set; }
        public DateOnly fecha_estimada { get; set; }
        public TimeOnly hora_llegada { get; set; }
    }
}