using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecmave.Api.Models
{
    [Table("recordatorios")]
    public class Recordatorio
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("UsuarioId")]
        public int UsuarioId { get; set; }

        [Column("VehiculoId")]
        public int VehiculoId { get; set; }

        [Column("FechaEnvio")]
        public DateTime FechaEnvio { get; set; }

        [Column("Tipo")]
        public string Tipo { get; set; } = string.Empty;
    }
}
