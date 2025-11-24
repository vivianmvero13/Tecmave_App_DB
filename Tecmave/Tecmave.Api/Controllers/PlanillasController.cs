using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanillasController : ControllerBase
    {
        private readonly PlanillasService _PlanillasService;

        public PlanillasController(PlanillasService PlanillasService)
        {
            _PlanillasService = PlanillasService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<PlanillasModel>> GetPlanillasModel()
        {
            return _PlanillasService.GetPlanillasModel();
        }

        [HttpGet("{id}")]
        public ActionResult<PlanillasModel> GetById(int id)
        {
            return _PlanillasService.GetByid_colaborador(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<PlanillasModel> AddPlanillas(PlanillasModel PlanillasModel)
        {

            var newPlanillasModel = _PlanillasService.AddPlanillas(PlanillasModel);

            return
                CreatedAtAction(
                        nameof(GetPlanillasModel), new
                        {
                            id = newPlanillasModel.id,
                        },
                        newPlanillasModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdatePlanillas(PlanillasModel PlanillasModel)
        {

            if (!_PlanillasService.UpdatePlanillas(PlanillasModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  colaborador no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeletePlanillasModel(int id)
        {

            if (!_PlanillasService.DeletePlanillas(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  colaborador no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }
    }
}
