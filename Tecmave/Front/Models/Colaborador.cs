using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Front.Models
{
    [Table("Colaboradores")]
    public class Colaborador
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        public string Apellido { get; set; }

        [Required, StringLength(15)]
        public string Cedula { get; set; }

        [EmailAddress]
        public string Correo { get; set; }

        public string Telefono { get; set; }

        [Required]
        public string Cargo { get; set; }

        [Required]
        public string Estado { get; set; }

        public DateTime FechaIngreso { get; set; } = DateTime.Now;
    }
}
