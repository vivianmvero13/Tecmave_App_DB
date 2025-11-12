using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecmave.Api.Models
{
    public class PromocionesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_promocion { get; set; }
        public string titulo { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public DateOnly fecha_inicio { get; set; }
        public DateOnly fecha_fin { get; set; }
        public int id_estado { get; set; }
        public bool recordatorio_enviado { get; set; } = false;
    }
}