namespace Tecmave.Api.Models.Dto
{
    public class ColaboradorInputModel
    {
        // Datos del usuario
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }

        // Datos del colaborador
        public string Puesto { get; set; }
        public decimal Salario { get; set; }

        // El cliente puede enviarla o se puede asignar automáticamente
        public DateOnly? FechaContratacion { get; set; }
    }
}
