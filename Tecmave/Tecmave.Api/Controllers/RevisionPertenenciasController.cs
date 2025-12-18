using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisionPertenenciasController : ControllerBase
    {

        private readonly RevisionPertenenciasService _revisionPertenenciaService;

        public RevisionPertenenciasController(RevisionPertenenciasService revisionPertenenciaService)
        {
            _revisionPertenenciaService = revisionPertenenciaService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<RevisionPertenenciasModel>> GetRevisionModel()
        {
            return _revisionPertenenciaService.GetRevisionPertenenciaModel();
        }

        [HttpGet("{id}")]
        public ActionResult<RevisionPertenenciasModel> GetById(int id)
        {
            return _revisionPertenenciaService.GetById(id);
        }

        [HttpGet("GetByIdRevision/{id_revision}")]
        public ActionResult<IEnumerable<RevisionPertenenciasModel>> GetByIdRevision(int id_revision)
        {
            var revisionPertenencias = _revisionPertenenciaService.GetPertenenciasByRevisionId(id_revision);
           
            return Ok(revisionPertenencias);
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

            var eliminado = _revisionPertenenciaService.DeletePorRevisionYNombre(revisionId, nombre);

            if (!eliminado)
            {
                return NotFound("La pertenencia no existe para esta revisión");
            }

            return Ok(new
            {
                mensaje = "Pertenencia eliminada correctamente"
            });
        }


        //Apis POST
        [HttpPost]
        public ActionResult<RevisionPertenenciasModel> AddRevision(RevisionPertenenciasModel RevisionDiagnosticoModel)
        {

            var newRevisionDiagnosticoModel = _revisionPertenenciaService.AddRevisionPertenencia(RevisionDiagnosticoModel);

            return
                CreatedAtAction(
                        nameof(GetRevisionModel), new
                        {
                            id = newRevisionDiagnosticoModel.id_pertenencia,
                        },
                        newRevisionDiagnosticoModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateRevision(RevisionPertenenciasModel RevisionModel)
        {

            if (!_revisionPertenenciaService.UpdateRevisionPertenencia(RevisionModel))
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

            if (!_revisionPertenenciaService.DeleteRevisionPertenencia(id))
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

