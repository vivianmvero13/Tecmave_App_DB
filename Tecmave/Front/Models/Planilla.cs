using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Front.Models
{
    public class Planilla
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ColaboradorId { get; set; }

        public Colaborador Colaborador { get; set; }

        [DataType(DataType.Date)]
        public DateTime PeriodoInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime PeriodoFin { get; set; }

        public decimal HorasTrabajadas { get; set; }

        public decimal ValorHora { get; set; }

        public decimal TotalSalario { get; set; }

        public decimal Deducciones { get; set; }

        public decimal NetoPagar { get; set; }

        public DateTime FechaGenerada { get; set; } = DateTime.Now;

        public string Estado { get; set; } = "Pendiente";

        [StringLength(250)]
        public string Observaciones { get; set; }
    }
}
