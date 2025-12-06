namespace Tecmave.Api.Models.Dto
{
    public class Colaborador
    {
        public ColaboradoresModel Colaboradores { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string UserName => Email;
        public string Email { get; set; }
        public string Rol { get; set; } = "Colaborador";
        public string Telefono { get; set; }
    }
}
