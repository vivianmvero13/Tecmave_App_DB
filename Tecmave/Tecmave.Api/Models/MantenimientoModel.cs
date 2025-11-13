using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecmave.Api.Models
{
    public class MantenimientoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMantenimiento { get; set; }
        public int IdVehiculo { get; set; }
        public Vehiculo? Vehiculo { get; set; }
        public DateOnly FechaMantenimiento { get; set; }
        public DateOnly? ProximoMantenimiento { get; set; }
        public bool RecordatorioEnviado { get; set; } = false;  
    }
}
