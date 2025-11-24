using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
  
    public class RevisionPertenenciasModel 
    {
        [Key]
        public int id_pertenencia { get; set; }
        public int revision_id { get; set; }
        public string nombre { get; set; } = string.Empty; // Ej: "Encendedor", "Radio"
        public bool presente { get; set; }

    }
}
