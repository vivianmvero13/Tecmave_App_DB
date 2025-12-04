namespace Tecmave.Front.Models
{
    public class PromocionesModel
    {
        public int id_promocion { get; set; }
        public string titulo { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public DateOnly fecha_inicio { get; set; }
        public DateOnly fecha_fin { get; set; }
        public int id_estado { get; set; }
        public string? imagen_url { get; set; }
        public bool recordatorio_enviado { get; set; }
    }
}
