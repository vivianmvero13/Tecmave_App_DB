using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class PlanillasModel
    {

        [Key]
        public int id { get; set; } // [pk, increment]
        public int colaborador_id { get; set; }
        public DateOnly periodo_inicio { get; set; }
        public DateOnly periodo_fin { get; set; }
        public decimal horas_trabajadas { get; set; }
        public decimal valor_hora { get; set; }
        public decimal total_salario { get; set; }
        
        public decimal neto_pagar { get; set; }
        public DateTime fecha_generada { get; set; }
        public string estado { get; set; }
        public string observaciones { get; set; }

    }


}
