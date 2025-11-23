using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("uploads")]
    public class UploadsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("promo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPromoImage([FromForm] PromoImagenUploadModel model)
        {
            var archivo = model.archivo;

            if (archivo == null || archivo.Length == 0)
                return BadRequest(new { mensaje = "No se recibió ningún archivo." });

            var ext = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            var permitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!permitidas.Contains(ext))
                return BadRequest(new { mensaje = "Tipo de archivo no permitido." });

            const long maxBytes = 5 * 1024 * 1024;
            if (archivo.Length > maxBytes)
                return BadRequest(new { mensaje = "El archivo supera el tamaño máximo permitido (5MB)." });

            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(webRoot))
                Directory.CreateDirectory(webRoot);

            var uploadsFolder = Path.Combine(webRoot, "promo-images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using (var stream = System.IO.File.Create(filePath))
                await archivo.CopyToAsync(stream);

            var urlPublica = $"{Request.Scheme}://{Request.Host}/promo-images/{fileName}";

            return Ok(new { url = urlPublica });
        }
    }
}
