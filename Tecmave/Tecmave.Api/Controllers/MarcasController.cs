using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarcasController : Controller
    {
        private readonly MarcasService _MarcasService;

        public MarcasController(MarcasService MarcasService)
        {
            _MarcasService = MarcasService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<MarcasModel>> GetMarcasModel()
        {
            return _MarcasService.GetMarcasModel();
        }

        [HttpGet("{id}")]
        public ActionResult<MarcasModel> GetById(int id)
        {
            return _MarcasService.GetByid_marca(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<MarcasModel> AddMarcas(MarcasModel MarcasModel)
        {

            var newMarcasModel = _MarcasService.AddMarcas(MarcasModel);

            return
                CreatedAtAction(
                        nameof(GetMarcasModel), new
                        {
                            id = newMarcasModel.id_marca,
                        },
                        newMarcasModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateMarcas(MarcasModel MarcasModel)
        {

            if (!_MarcasService.UpdateMarcas(MarcasModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  marca no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteMarcasModel(int id)
        {

            if (!_MarcasService.DeleteMarcas(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  marca no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
