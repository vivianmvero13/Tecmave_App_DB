using System;
using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class PromocionEnvio
    {
        [Key]
        public int IdEnvio{ get; set; }
        public int IdUsuario{ get; set; }
        public int IdPromocion { get; set; }
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;
    }
}
