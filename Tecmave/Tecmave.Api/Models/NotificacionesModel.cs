using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class NotificacionesModel
    {
        [Key]
        public int id_notificaciones { get; set; } // [pk, increment]
        public int usuario_id { get; set; }

        public int? id_promocion { get; set; }
        public string mensaje { get; set; }
        public DateOnly fecha_envio { get; set; }
        public string tipo { get; set; }
        public int id_estado { get; set; }
    }
}