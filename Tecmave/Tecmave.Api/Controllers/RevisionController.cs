using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevisionController : Controller
    {
        private readonly RevisionService _RevisionService;

        public RevisionController(RevisionService RevisionService)
        {
            _RevisionService = RevisionService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<RevisionModel>> GetRevisionModel()
        {
            return _RevisionService.GetRevisionModel();
        }

        [HttpGet("{id}")]
        public ActionResult<RevisionModel> GetById(int id)
        {
            return _RevisionService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<RevisionModel> AddRevision(RevisionModel RevisionModel)
        {

            var newRevisionModel = _RevisionService.AddRevision(RevisionModel);

            return
                CreatedAtAction(
                        nameof(GetRevisionModel), new
                        {
                            id = newRevisionModel.id_servicio,
                        },
                        newRevisionModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateRevision(RevisionModel RevisionModel)
        {

            if (!_RevisionService.UpdateRevision(RevisionModel))
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
        public IActionResult DeleteRevisionModel(int id)
        {

            if (!_RevisionService.DeleteRevision(id))
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
