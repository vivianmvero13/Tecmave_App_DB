using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class ServiciosModel
    {
        [Key]
        public int id_servicio { get; set; } // [pk, increment]
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string tipo { get; set; }
        public decimal precio { get; set; }
        public int tipo_servicio_id { get; set; }
    }
}