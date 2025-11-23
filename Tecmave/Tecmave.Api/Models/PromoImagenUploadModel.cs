using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Tecmave.Api.Models
{
    public class PromoImagenUploadModel
    {
        [Required]
        public IFormFile archivo { get; set; }
    }
}
