
namespace Tecmave.Api.Models.Dto
{
    public class GenerateFacturaFromServiceDto
    {
        public int cliente_id { get; set; }
        public int servicio_id { get; set; }
        public string? metodo_pago { get; set; }
    }
}
