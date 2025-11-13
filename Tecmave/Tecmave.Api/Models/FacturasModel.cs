using System;
using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class FacturasModel
    {
        [Key]
        public int id_factura { get; set; }
        public int? cliente_id { get; set; }
        public DateTime? fecha_emision { get; set; }
        public decimal? total { get; set; }
        public string? metodo_pago { get; set; }
        public string? estado_pago { get; set; } = "Pendiente";
    }
}
