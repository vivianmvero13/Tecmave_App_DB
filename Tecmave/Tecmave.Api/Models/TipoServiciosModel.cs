using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class TipoServiciosModel
    {
        [Key]
        public int id_tipo_servicio { get; set; } // [pk, increment]
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }
}