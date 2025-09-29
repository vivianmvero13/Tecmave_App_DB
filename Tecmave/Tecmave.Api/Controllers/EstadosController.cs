using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstadosController : Controller
    {
        private readonly EstadosService _EstadosService;

        public EstadosController(EstadosService EstadosService)
        {
            _EstadosService = EstadosService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<EstadosModel>> GetEstadosModel()
        {
            return _EstadosService.GetEstadosModel();
        }

        [HttpGet("{id}")]
        public ActionResult<EstadosModel> GetById(int id)
        {
            return _EstadosService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<EstadosModel> AddEstados(EstadosModel EstadosModel)
        {

            var newEstadosModel = _EstadosService.AddEstados(EstadosModel);

            return
                CreatedAtAction(
                        nameof(GetEstadosModel), new
                        {
                            id = newEstadosModel.id_estado,
                        },
                        newEstadosModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateEstados(EstadosModel EstadosModel)
        {

            if (!_EstadosService.UpdateEstados(EstadosModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de estado no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteEstadosModel(int id)
        {

            if (!_EstadosService.DeleteEstados(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de estado no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
