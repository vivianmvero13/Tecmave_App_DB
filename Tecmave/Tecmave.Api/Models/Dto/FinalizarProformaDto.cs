namespace Tecmave.Api.Models.Dto
{
    public class FinalizarProformaDto
    {
        public int id_revision { get; set; }
        public int id_estado { get; set; }
        public int? kilometraje { get; set; }
        public string? nivel_combustible { get; set; }

        public bool golpes_delantera { get; set; }
        public bool golpes_trasera { get; set; }
        public bool golpes_izquierda { get; set; }
        public bool golpes_derecha { get; set; }
        public bool golpes_arriba { get; set; }

        public bool vehiculo_sucio { get; set; }
        public bool vehiculo_mojado { get; set; }

        public DateTime? fecha_estimada_entrega { get; set; }
        public DateTime? fecha_entrega_final { get; set; }

        public string? notas_taller { get; set; }
    }
}
