using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgendamientoController : Controller
    {
        private readonly AgendamientoService _agendamientoService;

        public AgendamientoController(AgendamientoService agendamientoService)
        {
            _agendamientoService = agendamientoService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<AgendamientoModel>> GetAgendamientoModel()
        {
            return _agendamientoService.GetAgendamientoModel();
        }

        [HttpGet("{id}")]
        public ActionResult<AgendamientoModel> GetById(int id)
        {
            return _agendamientoService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<AgendamientoModel> AddAgendamiento(AgendamientoModel agendamientoModel)
        {

            var newAgendamientoModel = _agendamientoService.AddAgendamiento(agendamientoModel);

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
        public IActionResult UpdateAgendamiento(AgendamientoModel agendamientoModel)
        {

            if (!_agendamientoService.UpdateAgendamiento(agendamientoModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El agendamiento no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteAgendamientoModel(int id)
        {

            if (!_agendamientoService.DeleteAgendamiento(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El agendamiento no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
