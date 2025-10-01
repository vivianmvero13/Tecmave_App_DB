using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResenasController : Controller
    {
        private readonly ResenasService _ResenasService;

        public ResenasController(ResenasService ResenasService)
        {
            _ResenasService = ResenasService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<ResenasModel>> GetResenasModel()
        {
            return _ResenasService.GetResenasModel();
        }

        [HttpGet("{id}")]
        public ActionResult<ResenasModel> GetById(int id)
        {
            return _ResenasService.GetByid_resena(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<ResenasModel> AddResenas(ResenasModel ResenasModel)
        {

            var newResenasModel = _ResenasService.AddResenas(ResenasModel);

            return
                CreatedAtAction(
                        nameof(GetResenasModel), new
                        {
                            id = newResenasModel.id_resena,
                        },
                        newResenasModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateResenas(ResenasModel ResenasModel)
        {

            if (!_ResenasService.UpdateResenas(ResenasModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  resena no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteResenasModel(int id)
        {

            if (!_ResenasService.DeleteResenas(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  resena no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
