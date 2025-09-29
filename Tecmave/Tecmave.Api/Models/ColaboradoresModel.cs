using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class ColaboradoresModel
    {
        [Key]
        public int id_colaborador { get; set; } // [pk, increment]
        public int id_usuario { get; set; }
        public string puesto { get; set; }
        public decimal salario { get; set; }
        public DateOnly fecha_contratacion { get; set; }
    }
}