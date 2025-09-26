using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _UsuariosService;

        public UsuariosController(UsuarioService UsuariosService)
        {
            _UsuariosService = UsuariosService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<UsuariosModel>> GetUsuariosModel()
        {
            return _UsuariosService.GetUsuariosModel();
        }

        [HttpGet("{id}")]
        public ActionResult<UsuariosModel> GetById(int id)
        {
            return _UsuariosService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<UsuariosModel> AddUsuarios(UsuariosModel UsuariosModel)
        {

            var newUsuariosModel = _UsuariosService.AddUsuario(UsuariosModel);

            return
                CreatedAtAction(
                        nameof(GetUsuariosModel), new
                        {
                            id = newUsuariosModel.Id,
                        },
                        newUsuariosModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateUsuarios(UsuariosModel UsuariosModel)
        {

            if (!_UsuariosService.UpdateUsuario(UsuariosModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de usuario no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteUsuariosModel(int id)
        {

            if (!_UsuariosService.DeleteUsuario(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de usuario no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
