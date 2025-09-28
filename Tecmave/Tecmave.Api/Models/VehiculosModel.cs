using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class VehiculosModel
    {
        [Key]
        public int id_vehiculo { get; set; } // [pk, increment]
        public int cliente_id { get; set; }
        public int id_marca { get; set; } // [pk, increment]
        public int anio { get; set; }
        public string placa { get; set; }
    }
}