using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("promociones")]
    public class PromocionesController : ControllerBase
    {
        private readonly PromocionesService _promocionesService;

        public PromocionesController(PromocionesService promocionesService)
        {
            _promocionesService = promocionesService;
        }

        // GET /promociones
        [HttpGet]
        public ActionResult<IEnumerable<PromocionesModel>> GetPromocionesModel()
        {
            var promos = _promocionesService.GetPromocionesModel();
            return Ok(promos);
        }

        // GET /promociones/5
        [HttpGet("{id:int}")]
        public ActionResult<PromocionesModel> GetById(int id)
        {
            var promo = _promocionesService.GetByid_promocion(id);
            if (promo == null)
                return NotFound();

            return Ok(promo);
        }

        // POST /promociones
        [HttpPost]
        public ActionResult<PromocionesModel> AddPromociones([FromBody] PromocionesModel model)
        {
            var nueva = _promocionesService.AddPromociones(model);

            return CreatedAtAction(
                nameof(GetById),
                new { id = nueva.id_promocion },
                nueva
            );
        }

        // PUT /promociones
        [HttpPut]
        public IActionResult UpdatePromociones([FromBody] PromocionesModel model)
        {
            if (!_promocionesService.UpdatePromociones(model))
            {
                return NotFound(new { mensaje = "La promoción no fue encontrada" });
            }

            return NoContent();
        }

        // DELETE /promociones/5
        [HttpDelete("{id:int}")]
        public IActionResult DeletePromocionesModel(int id)
        {
            if (!_promocionesService.DeletePromociones(id))
            {
                return NotFound(new { mensaje = "La promoción no fue encontrada" });
            }

            return NoContent();
        }

        // POST /promociones/enviar-recordatorios
        [HttpPost("enviar-recordatorios")]
        public async Task<IActionResult> EnviarRecordatorios()
        {
            await _promocionesService.EnviarRecordatoriosAsync();
            return Ok(new { mensaje = "Recordatorios enviados correctamente." });
        }

        // POST /promociones/enviar-promo/5
        [HttpPost("enviar-promo/{idPromocion:int}")]
        public async Task<IActionResult> EnviarPromocionPorPromo(int idPromocion)
        {
            var cantidad = await _promocionesService.EnviarPromocionMasivoPorPromoAsync(idPromocion);

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
