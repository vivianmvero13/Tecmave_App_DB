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

