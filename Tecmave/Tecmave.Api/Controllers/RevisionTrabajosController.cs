using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisionTrabajosController : ControllerBase
    {
        private readonly RevisionTrabajosService _revisionTrabajoService;

        public RevisionTrabajosController(RevisionTrabajosService revisionDiagnosticoService)
        {
            _revisionTrabajoService = revisionDiagnosticoService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<RevisionTrabajosModel>> GetRevisionModel()
        {
            return _revisionTrabajoService.GetRevisionTrabajoModel();
        }

        [HttpGet("{id}")]
        public ActionResult<RevisionTrabajosModel> GetById(int id)
        {
            return _revisionTrabajoService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<RevisionTrabajosModel> AddRevision(RevisionTrabajosModel RevisionDiagnosticoModel)
        {

            var newRevisionDiagnosticoModel = _revisionTrabajoService.AddRevisionTrabajo(RevisionDiagnosticoModel);

            return
                CreatedAtAction(
                        nameof(GetRevisionModel), new
                        {
                            id = newRevisionDiagnosticoModel.id_trabajo,
                        },
                        newRevisionDiagnosticoModel);

        }

        [HttpGet("GetByIdRevision/{id_revision}")]
        public ActionResult<IEnumerable<RevisionTrabajosModel>> GetByIdRevision(int id_revision)
        {
            var revisionTrabajoss = _revisionTrabajoService.GetTrabajosByRevisionId(id_revision);
            if (revisionTrabajoss == null)
            {
                return NotFound(new
                {
                    mensaje = "La revisión no existe"
                });
            }

            return Ok(revisionTrabajoss); // puede venir vacío
        }


            [HttpDelete("Por-revision")]
        public IActionResult DeletePorRevisionYNombre(
        [FromQuery] int revisionId,
        [FromQuery] string nombre)
        {
            if (revisionId <= 0 || string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("Parámetros inválidos");
            }

            var eliminado = _revisionTrabajoService.DeletePorRevisionYNombre(revisionId, nombre);

            if (!eliminado)
            {
                return NotFound("El trabajo no existe para esta revisión");
            }

            return Ok(new
            {
                mensaje = "Trabajo eliminado correctamente"
            });
        }



        //APIS PUT
        [HttpPut]
        public IActionResult UpdateRevision(RevisionTrabajosModel RevisionModel)
        {

            if (!_revisionTrabajoService.UpdateRevisionTrabajo(RevisionModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de servicio no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteRevisionDiagnosticoModel(int id)
        {

            if (!_revisionTrabajoService.DeleteRevisionTrabajo(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de servicio no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
