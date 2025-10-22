using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class ModelosModel
    {
        [Key]
        public int id_modelo { get; set; } // [pk, increment]
        public string nombre { get; set; }
        public int id_marca{ get; set; }
    }
}