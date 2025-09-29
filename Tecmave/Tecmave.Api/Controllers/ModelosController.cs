using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModelosController : Controller
    {
        private readonly ModelosService _ModelosService;

        public ModelosController(ModelosService ModelosService)
        {
            _ModelosService = ModelosService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<ModelosModel>> GetModelosModel()
        {
            return _ModelosService.GetModelosModel();
        }

        [HttpGet("{id}")]
        public ActionResult<ModelosModel> GetById(int id)
        {
            return _ModelosService.GetByid_modelo(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<ModelosModel> AddModelos(ModelosModel ModelosModel)
        {

            var newModelosModel = _ModelosService.AddModelos(ModelosModel);

            return
                CreatedAtAction(
                        nameof(GetModelosModel), new
                        {
                            id = newModelosModel.id_modelo,
                        },
                        newModelosModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateModelos(ModelosModel ModelosModel)
        {

            if (!_ModelosService.UpdateModelos(ModelosModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de modelo no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteModelosModel(int id)
        {

            if (!_ModelosService.DeleteModelos(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de modelo no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
