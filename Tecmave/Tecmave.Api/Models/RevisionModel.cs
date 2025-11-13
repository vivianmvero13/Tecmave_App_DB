using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class RevisionModel
    {
        [Key]
        public int id_revision { get; set; } // [pk, increment]

        public int vehiculo_id { get; set; }

        public DateTime fecha_ingreso { get; set; }

        public int id_servicio { get; set; }

        public int id_estado { get; set; }

        public DateTime? fecha_estimada_entrega { get; set; }   // <-- NULL permitido
        public DateTime? fecha_entrega_final { get; set; }      // <-- NULL permitido

        public int id_agendamiento { get; set; }

        public int? kilometraje { get; set; }                   // <-- NULL permitido

        public string? observaciones_cliente { get; set; }      // <-- NULL permitido

        public string? nivel_combustible { get; set; }          // <-- NULL permitido

        public bool golpes_delantera { get; set; }
        public bool golpes_trasera { get; set; }
        public bool golpes_izquierda { get; set; }
        public bool golpes_derecha { get; set; }
        public bool golpes_arriba { get; set; }

        public bool vehiculo_sucio { get; set; }
        public bool vehiculo_mojado { get; set; }

        public string? notas_taller { get; set; } = string.Empty;

    }
}