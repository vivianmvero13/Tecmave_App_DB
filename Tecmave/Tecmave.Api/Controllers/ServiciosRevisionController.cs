using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiciosRevisionController : Controller
    {
        private readonly ServiciosRevisionService _ServiciosRevisionService;

        public ServiciosRevisionController(ServiciosRevisionService ServiciosRevisionService)
        {
            _ServiciosRevisionService = ServiciosRevisionService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<ServiciosRevisionModel>> GetServiciosRevisionModel()
        {
            return _ServiciosRevisionService.GetServiciosRevisionModel();
        }

        [HttpGet("{id}")]
        public ActionResult<ServiciosRevisionModel> GetById(int id)
        {
            return _ServiciosRevisionService.GetByid_servicios_revision(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<ServiciosRevisionModel> AddServiciosRevision(ServiciosRevisionModel ServiciosRevisionModel)
        {

            var newServiciosRevisionModel = _ServiciosRevisionService.AddServiciosRevision(ServiciosRevisionModel);

            return
                CreatedAtAction(
                        nameof(GetServiciosRevisionModel), new
                        {
                            id = newServiciosRevisionModel.id_servicio_revision,
                        },
                        newServiciosRevisionModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateServiciosRevision(ServiciosRevisionModel ServiciosRevisionModel)
        {

            if (!_ServiciosRevisionService.UpdateServiciosRevision(ServiciosRevisionModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El servicio revision no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteServiciosRevisionModel(int id)
        {

            if (!_ServiciosRevisionService.DeleteServiciosRevision(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El servicio revision no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
