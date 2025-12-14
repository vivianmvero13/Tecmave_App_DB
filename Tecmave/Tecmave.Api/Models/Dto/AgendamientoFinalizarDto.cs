namespace Tecmave.Api.Models.Dto
{
    public class AgendamientoFinalizarDto
    {
        public int id_agendamiento { get; set; }

        public int vehiculo_id { get; set; }

        public DateOnly fecha_agregada { get; set; }

        public DateOnly fecha_estimada { get; set; }

        public DateOnly fecha_estimada_entrega { get; set; }

        public TimeOnly hora_llegada { get; set; }
    }
}
