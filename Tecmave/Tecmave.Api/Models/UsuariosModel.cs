using System.ComponentModel.DataAnnotations;

namespace Tecmave.Api.Models
{
    public class UsuariosModel
    {
        [Key]
        public int Id { get; set; } // [pk, increment]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
    }
}