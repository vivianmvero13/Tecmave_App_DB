using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColaboradoresController : Controller
    {
        private readonly ColaboradoresService _ColaboradoresService;

        public ColaboradoresController(ColaboradoresService ColaboradoresService)
        {
            _ColaboradoresService = ColaboradoresService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<ColaboradoresModel>> GetColaboradoresModel()
        {
            return _ColaboradoresService.GetColaboradoresModel();
        }

        [HttpGet("{id}")]
        public ActionResult<ColaboradoresModel> GetById(int id)
        {
            return _ColaboradoresService.GetByid_colaborador(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<ColaboradoresModel> AddColaboradores(ColaboradoresModel ColaboradoresModel)
        {

            var newColaboradoresModel = _ColaboradoresService.AddColaboradores(ColaboradoresModel);

            return
                CreatedAtAction(
                        nameof(GetColaboradoresModel), new
                        {
                            id = newColaboradoresModel.id_colaborador,
                        },
                        newColaboradoresModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateColaboradores(ColaboradoresModel ColaboradoresModel)
        {

            if (!_ColaboradoresService.UpdateColaboradores(ColaboradoresModel))
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
        public IActionResult DeleteColaboradoresModel(int id)
        {

            if (!_ColaboradoresService.DeleteColaboradores(id))
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
