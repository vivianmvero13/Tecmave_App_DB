using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class PromocionesModel
    {
        [Key]
        public int id_promocion { get; set; } // [pk, increment]
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateOnly fecha_inicio { get; set; }
        public DateOnly fecha_fin { get; set; }
        public int id_estado { get; set; }
    }
}