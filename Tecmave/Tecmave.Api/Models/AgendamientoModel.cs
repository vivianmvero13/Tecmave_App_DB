namespace Tecmave.Api.Models
{
    public class AgendamientoModel
    {
        public int id_agendamiento { get; set; } // [pk, increment]
        public string cliente_id { get; set; }// [fk, not null]
        public int vehiculo_id { get; set; } // [fk, not null]
        public string fecha_agregada { get; set; }
        public string estado {  get; set; }
    }
}