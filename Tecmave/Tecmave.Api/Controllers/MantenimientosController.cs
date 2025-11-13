using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MantenimientosController : Controller
    {
        private readonly MantenimientoService _mantenimientosService;

        public MantenimientosController(MantenimientoService mantenimientosService)
        {
            _mantenimientosService = mantenimientosService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MantenimientoModel>> GetMantenimientos()
        {
            return _mantenimientosService.GetMantenimientoModels();
        }

        [HttpGet("{id}")]
        public ActionResult<MantenimientoModel> GetById(int id)
        {
            var m = _mantenimientosService.GetById(id);
            if (m == null)
                return NotFound();

            return m;
        }

        [HttpPost]
        public ActionResult<MantenimientoModel> AddMantenimiento(MantenimientoModel mantenimiento)
        {
            var nuevo = _mantenimientosService.AddMantenimiento(mantenimiento);

            return CreatedAtAction(nameof(GetById),
                new { id = nuevo.IdMantenimiento },
                nuevo);
        }

        [HttpPut]
        public IActionResult UpdateMantenimiento(MantenimientoModel mantenimiento)
        {
            if (!_mantenimientosService.UpdateMantenimiento(mantenimiento))
                return NotFound(new { mensaje = "Mantenimiento no encontrado" });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMantenimiento(int id)
        {
            if (!_mantenimientosService.DeleteMantenimiento(id))
                return NotFound(new { mensaje = "Mantenimiento no encontrado" });

            return NoContent();
        }

        // 🔹 MASIVO: el que ya tenías
        [HttpPost("Enviar-recordatorios")]
        public async Task<IActionResult> EnviarRecordatorios()
        {
            await _mantenimientosService.EnviarRecordatorioAsync();
            return Ok(new { mensaje = "Recordatorios enviados" });
        }

        // 🔹 NUEVO: individual por mantenimiento
        [HttpPost("{id}/enviar-recordatorio")]
        public async Task<IActionResult> EnviarRecordatorioIndividual(int id)
        {
            var ok = await _mantenimientosService.EnviarRecordatorioIndividualAsync(id);

            if (!ok)
                return NotFound(new { mensaje = "Mantenimiento no encontrado o sin cliente asociado." });

            return Ok(new { mensaje = "Recordatorio enviado correctamente." });
        }
    }
}
