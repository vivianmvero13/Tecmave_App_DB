using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecmave.Api.Models
{
    [Table("promociones")]
    public class PromocionesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_promocion")]
        public int id_promocion { get; set; }

        [Column("titulo")]
        public string titulo { get; set; } = string.Empty;

        [Column("descripcion")]
        public string descripcion { get; set; } = string.Empty;

        [Column("fecha_inicio")]
        public DateOnly fecha_inicio { get; set; }

        [Column("fecha_fin")]
        public DateOnly fecha_fin { get; set; }

        [Column("id_estado")]
        public int id_estado { get; set; }

        [Column("recordatorio_enviado")]
        public bool recordatorio_enviado { get; set; } = false;
    }
}
