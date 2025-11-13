using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class RevisionTrabajosModel
    {
        [Key]
        public int id_trabajo { get; set; }
        public int revision_id { get; set; }
        public string nombre { get; set; } = string.Empty; // Ej: "Diagnóstico 0 M.", "Diagnóstico sist. frenos"
        public bool realizado { get; set; }

    }
}
