using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResenasController : ControllerBase
    {
        private readonly ResenasService _svc;
        public ResenasController(ResenasService svc) => _svc = svc;

        public record CrearResenaDto(int servicio_id, string comentario, float calificacion);

        // GET: Reseñas públicas
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ResenasModel>>> GetResenas()
            => Ok(await _svc.GetResenasAsync());

        // GET: /Resenas/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResenasModel>> GetById(int id)
        {
            var item = await _svc.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        // POST: crear reseña (solo Cliente)
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult<ResenasModel>> Create([FromBody] CrearResenaDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Doble verificación defensiva
            if (!User.IsInRole("Cliente")) return Forbid();

            var nuevo = new ResenasModel
            {
                cliente_id = userId,
                servicio_id = dto.servicio_id,
                comentario = dto.comentario,
                calificacion = dto.calificacion,
                fecha = DateTime.UtcNow
            };

            var creado = await _svc.AddAsync(nuevo);
            return CreatedAtAction(nameof(GetById), new { id = creado.id_resena }, creado);
        }

        // PUT: actualizar comentario (puedes restringir a dueño o admins si quieres)
        [HttpPut("{id:int}/comentario")]
        [Authorize] // opcional: Roles = "Cliente,Administrador"
        public async Task<IActionResult> UpdateComentario(int id, [FromBody] string comentario)
        {
            var ok = await _svc.UpdateComentarioAsync(id, comentario);
            return ok ? NoContent() : NotFound(new { mensaje = "La reseña no fue encontrada" });
        }

        // DELETE
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")] // sugerido: solo admin borra
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _svc.DeleteAsync(id);
            return ok ? NoContent() : NotFound(new { mensaje = "La reseña no fue encontrada" });
        }
    }
}
