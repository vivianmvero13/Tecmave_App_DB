using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiciosController : Controller
    {
        private readonly ServiciosService _ServiciosService;

        public ServiciosController(ServiciosService ServiciosService)
        {
            _ServiciosService = ServiciosService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<ServiciosModel>> GetServiciosModel()
        {
            return _ServiciosService.GetServiciosModel();
        }

        [HttpGet("{id}")]
        public ActionResult<ServiciosModel> GetById(int id)
        {
            return _ServiciosService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<ServiciosModel> AddServicios(ServiciosModel ServiciosModel)
        {

            var newServiciosModel = _ServiciosService.AddServicios(ServiciosModel);

            return
                CreatedAtAction(
                        nameof(GetServiciosModel), new
                        {
                            id = newServiciosModel.id_servicio,
                        },
                        newServiciosModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateServicios(ServiciosModel ServiciosModel)
        {

            if (!_ServiciosService.UpdateServicios(ServiciosModel))
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
        public IActionResult DeleteServiciosModel(int id)
        {

            if (!_ServiciosService.DeleteServicios(id))
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
