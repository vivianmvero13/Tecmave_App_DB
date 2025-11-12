using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromocionesController : Controller
    {
        private readonly PromocionesService _PromocionesService;

        public PromocionesController(PromocionesService PromocionesService)
        {
            _PromocionesService = PromocionesService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<PromocionesModel>> GetPromocionesModel()
        {
            return _PromocionesService.GetPromocionesModel();
        }

        [HttpGet("{id}")]
        public ActionResult<PromocionesModel> GetById(int id)
        {
            return _PromocionesService.GetByid_promocion(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<PromocionesModel> AddPromociones(PromocionesModel PromocionesModel)
        {

            var newPromocionesModel = _PromocionesService.AddPromociones(PromocionesModel);

            return
                CreatedAtAction(
                        nameof(GetPromocionesModel), new
                        {
                            id = newPromocionesModel.id_promocion,
                        },
                        newPromocionesModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdatePromociones(PromocionesModel PromocionesModel)
        {

            if (!_PromocionesService.UpdatePromociones(PromocionesModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  promocion no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeletePromocionesModel(int id)
        {

            if (!_PromocionesService.DeletePromociones(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  promocion no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        // Envio de los recordatorios de las promociones

        [HttpPost("enviar-recordatorios")]

        public async Task<IActionResult> EnviarRecordatorios()
        {
            await _PromocionesService.EnviarRecordatoriosAsync();
            return Ok(new { mensaje = "Recordatorios enviados correctamente." });
        }

        [HttpPost("enviar/{idUsuario}")]
        public async Task<IActionResult> EnviarPromociones(int idUsuario, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { mensaje = "Debe proporcionar un email de destino." });

            var cantidad = await _PromocionesService.EnviarPromocion(idUsuario, email);

            if (cantidad == 0)
                return Ok(new { mensaje = "No hay promociones nuevas para enviar o ya fueron enviadas." });

            return Ok(new { mensaje = $"Se enviaron {cantidad} promociones activas al usuario {idUsuario}." });
        }
    }
}
