using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecmave.Api.Models
{
    [Table("vehiculos")] 
    public class Vehiculo
    {
        [Key]
        [Column("id_vehiculo")]
        public int IdVehiculo { get; set; }

        [Column("cliente_id")]
        public int ClienteId { get; set; }

        public Usuario? Cliente { get; set; }

        [Column("id_marca")]
        public int IdMarca { get; set; }

        [Column("anno")]
        public int Anno { get; set; }

        [Column("modelo")]
        public string? Modelo { get; set; }

        [Column("placa")]
        public string Placa { get; set; } = string.Empty;

        public DateOnly? Proximo { get; set; }
    }
}
