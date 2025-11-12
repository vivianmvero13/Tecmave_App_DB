namespace Tecmave.Front.Models
{
    public class PromocionesDto
    {
        public int IdPromocion { get; set; } 
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int IdEstado { get; set; }
    }
}
