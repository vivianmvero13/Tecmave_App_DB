using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class RolesModel
    {
        [Key]
        public int Id { get; set; } // [pk, increment]
        public string Name { get; set; }
    }
}