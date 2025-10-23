using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class VehiculosModel
    {
        [Key]
        public int id_vehiculo { get; set; } // [pk, increment]
        public int cliente_id { get; set; }
        public int id_marca { get; set; } // [pk, increment]
        public int anno { get; set; }
        public string placa { get; set; }

        public string modelo { get; set; }

    }
}