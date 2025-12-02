using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResenasController : Controller
    {
        private readonly ResenasService _resenasService;

        public ResenasController(ResenasService resenasService)
        {
            _resenasService = resenasService;
        }

        // ============================================
        // GET CRUD BASICO
        // ============================================
        [HttpGet]
        public ActionResult<IEnumerable<ResenasModel>> GetResenasModel()
        {
            return Ok(_resenasService.GetResenasModel());
        }

        [HttpGet("{id}")]
        public ActionResult<ResenasModel> GetById(int id)
        {
            var r = _resenasService.GetByid_resena(id);

            if (r == null)
                return NotFound("No se encontró la reseña.");

            return Ok(r);
        }


        // ============================================
        // GET RESEÑAS PÚBLICAS (JOIN COMPLETO)
        // ============================================
        [HttpGet("Publicas")]
        public ActionResult GetPublicas()
        {
            return Ok(_resenasService.GetResenasPublicas());
        }


        // ============================================
        // POST — AGREGAR RESEÑA CON VALIDACIONES
        // ============================================
        [HttpPost]
        public ActionResult AddResenas([FromBody] ResenasModel model)
        {
            var result = _resenasService.AgregarConValidacion(model);

            if (!result.ok)
                return BadRequest(result.error);

            return CreatedAtAction(nameof(GetById),
                new { id = result.nueva!.id_resena },
                result.nueva);
        }


        // ============================================
        // PUT — EDITAR RESEÑA
        // ============================================
        [HttpPut]
        public IActionResult UpdateResenas([FromBody] ResenasModel model)
        {
            if (!_resenasService.UpdateResenas(model))
                return NotFound("La reseña no fue encontrada.");

            return NoContent();
        }


        // ============================================
        // DELETE — ELIMINAR RESEÑA
        // ============================================
        [HttpDelete("{id}")]
        public IActionResult DeleteResenasModel(int id)
        {
            if (!_resenasService.DeleteResenas(id))
                return NotFound("La reseña no fue encontrada.");

            return NoContent();
        }
    }
}
