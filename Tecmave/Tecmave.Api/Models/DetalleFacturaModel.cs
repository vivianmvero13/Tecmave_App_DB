using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class DetalleFacturaModel
    {
        [Key]
        public int id_detalle { get; set; } // [pk, increment]
        public int factura_id { get; set; }
        public int servicio_id { get; set; }
        public string descripcion { get; set; }
        public decimal costo { get; set; }
        public decimal subtotal { get; set; }
    }
}