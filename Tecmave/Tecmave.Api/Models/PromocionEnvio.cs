using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecmave.Api.Models
{
    [Table("promocion_envios")]
    public class PromocionEnvio
    {
        [Key]
        [Column("id_envio")]
        public int IdEnvio { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_promocion")]
        public int IdPromocion { get; set; }

        [Column("fecha_envio")]
        public DateTime FechaEnvio { get; set; }
    }
}
