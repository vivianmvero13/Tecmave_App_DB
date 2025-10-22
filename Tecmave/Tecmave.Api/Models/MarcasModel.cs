using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class MarcasModel
    {
        [Key]
        public int id_marca { get; set; } // [pk, increment]
        public string nombre { get; set; }
    }
}