namespace Tecmave.Api.Models.Dto
{
    public class EditarColaboradorDto
    {
        public int IdColaborador { get; set; }
        public string Puesto { get; set; }
        public decimal Salario { get; set; }
        public DateTime FechaContratacion { get; set; }

        // Usuario
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        

      
    }
}
