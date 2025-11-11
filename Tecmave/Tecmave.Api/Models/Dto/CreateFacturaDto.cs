
namespace Tecmave.Api.Models.Dto
{
    public class CreateFacturaDto
    {
        public int cliente_id { get; set; }
        public System.DateTime? fecha_emision { get; set; }
        public decimal total { get; set; }
        public string? metodo_pago { get; set; }
        public string? estado_pago { get; set; } = "Pendiente";
    }
}
