// Tecmave.Api/Models/Vehiculo.cs
namespace Tecmave.Api.Models
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }
        public int? ClienteId { get; set; }   // FK a aspnetusers.Id
        public int IdMarca { get; set; }      // FK a marca.id_marca

        public int Anno { get; set; } 
        public string Placa { get; set; } = "";
        public string? Modelo { get; set; }
    }
}