using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoServiciosController : Controller
    {
        private readonly TipoServiciosService _tipoServiciosService;

        public TipoServiciosController(TipoServiciosService tipoServiciosService)
        {
            _tipoServiciosService = tipoServiciosService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<TipoServiciosModel>> GetTipoServiciosModel()
        {
            return _tipoServiciosService.GetTipoServiciosModel();
        }

        [HttpGet("{id}")]
        public ActionResult<TipoServiciosModel> GetById(int id)
        {
            return _tipoServiciosService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<TipoServiciosModel> AddTipoServicios(TipoServiciosModel tipoServiciosModel)
        {

            var newTipoServiciosModel = _tipoServiciosService.AddTipoServicios(tipoServiciosModel);

            return
                CreatedAtAction(
                        nameof(GetTipoServiciosModel), new
                        {
                            id = newTipoServiciosModel.id_tipo_servicio,
                        },
                        newTipoServiciosModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateTipoServicios(TipoServiciosModel tipoServiciosModel)
        {

            if (!_tipoServiciosService.UpdateTipoServicios(tipoServiciosModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El tipo de servicio no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteTipoServiciosModel(int id)
        {

            if (!_tipoServiciosService.DeleteTipoServicios(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El tipo de servicio no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
