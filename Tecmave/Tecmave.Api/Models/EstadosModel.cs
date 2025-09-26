using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class EstadosModel
    {
        [Key]
        public int id_estado { get; set; } // [pk, increment]
        public string nombre { get; set; }
    }
}