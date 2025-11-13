using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromocionesController : Controller
    {
        private readonly PromocionesService _PromocionesService;

        public PromocionesController(PromocionesService promocionesService)
        {
            _PromocionesService = promocionesService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PromocionesModel>> GetPromocionesModel()
        {
            return _PromocionesService.GetPromocionesModel();
        }

        [HttpGet("{id}")]
        public ActionResult<PromocionesModel> GetById(int id)
        {
            var promo = _PromocionesService.GetByid_promocion(id);
            if (promo == null)
                return NotFound();

            return promo;
        }

        [HttpPost]
        public ActionResult<PromocionesModel> AddPromociones(PromocionesModel model)
        {
            var nueva = _PromocionesService.AddPromociones(model);

            return CreatedAtAction(
                nameof(GetPromocionesModel),
                new { id = nueva.id_promocion },
                nueva);
        }

        [HttpPut]
        public IActionResult UpdatePromociones(PromocionesModel model)
        {
            if (!_PromocionesService.UpdatePromociones(model))
            {
                return NotFound(new { mensaje = "La promoción no fue encontrada" });
            }

            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeletePromocionesModel(int id)
        {
            if (!_PromocionesService.DeletePromociones(id))
            {
                return NotFound(new { mensaje = "La promoción no fue encontrada" });
            }

            return NoContent();
        }

        [HttpPost("enviar-recordatorios")]
        public async Task<IActionResult> EnviarRecordatorios()
        {
            await _PromocionesService.EnviarRecordatoriosAsync();
            return Ok(new { mensaje = "Recordatorios enviados correctamente." });
        }

        [HttpPost("enviar-promo/{idPromocion}")]
        public async Task<IActionResult> EnviarPromocionPorPromo(int idPromocion)
        {
            var cantidad = await _PromocionesService.EnviarPromocionMasivoPorPromoAsync(idPromocion);

            if (cantidad == 0)
            {
                return Ok(new
                {
                    mensaje = "No se enviaron correos. Puede que no haya usuarios suscritos, la promoción no esté activa o ya se haya enviado a todos."
                });
            }

            return Ok(new { mensaje = $"Se enviaron {cantidad} correos para la promoción {idPromocion}." });
        }
    }
}
