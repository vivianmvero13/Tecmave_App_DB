namespace Tecmave.Api.Models
{
    public class Recordatorio
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public int VehiculoId { get; set; }

        public DateTime FechaEnvio { get; set; }
        public string Tipo { get; set; } = "Semestre";
    }
}
