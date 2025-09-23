using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgendamientoController : Controller
    {
        private readonly AgendamientoService _cantonService;

        public AgendamientoController(AgendamientoService cantonService)
        {
            _cantonService = cantonService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<AgendamientoModel>> GetAgendamientoModel()
        {
            return _cantonService.GetAgendamientoModel();
        }

        [HttpGet("{id}")]
        public ActionResult<AgendamientoModel> GetById(int id)
        {
            return _cantonService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<AgendamientoModel> AddAgendamiento(AgendamientoModel cantonModel)
        {

            var newAgendamientoModel = _cantonService.AddAgendamiento(cantonModel);

            return
                CreatedAtAction(
                        nameof(GetAgendamientoModel), new
                        {
                            id = newAgendamientoModel.id_agendamiento,
                        },
                        newAgendamientoModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateAgendamiento(AgendamientoModel cantonModel)
        {

            if (!_cantonService.UpdateAgendamiento(cantonModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El canton no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteAgendamientoModel(int id)
        {

            if (!_cantonService.DeleteAgendamiento(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El canton no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
