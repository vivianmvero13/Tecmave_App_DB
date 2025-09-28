using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class MarcasModel
    {
        [Key]
        public int id_marca { get; set; } // [pk, increment]
        public int id_modelo { get; set; }
        public string nombre { get; set; }
    }
}