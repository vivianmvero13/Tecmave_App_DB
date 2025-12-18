namespace Tecmave.Api.Models.Dto
{
    public class ColaboradorInputModel
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; } = "Colaborador";
        public string Puesto { get; set; }
        public decimal Salario { get; set; }
        public DateOnly? FechaContratacion { get; set; }
    }
}
