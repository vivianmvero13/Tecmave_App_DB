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
            var lista = _mantenimientosService.GetMantenimientoModels();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public ActionResult<MantenimientoModel> GetById(int id)
        {
            var m = _mantenimientosService.GetById(id);
            if (m == null)
                return NotFound(new { mensaje = "Mantenimiento no encontrado" });

            return Ok(m);
        }

        [HttpPost]
        public ActionResult<MantenimientoModel> AddMantenimiento([FromBody] MantenimientoModel mantenimiento)
        {
            var nuevo = _mantenimientosService.AddMantenimiento(mantenimiento);

            return CreatedAtAction(nameof(GetById),
                new { id = nuevo.IdMantenimiento },
                nuevo);
        }

        [HttpPut]
        public IActionResult UpdateMantenimiento([FromBody] MantenimientoModel mantenimiento)
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

        [HttpPost("Enviar-recordatorios")]
        public async Task<IActionResult> EnviarRecordatorios()
        {
            await _mantenimientosService.EnviarRecordatorioAsync();
            return Ok(new { mensaje = "Recordatorios enviados" });
        }

        [HttpPost("{id}/enviar-recordatorio")]
        public async Task<IActionResult> EnviarRecordatorioIndividual(int id)
        {
            var ok = await _mantenimientosService.EnviarRecordatorioIndividualAsync(id);

            if (!ok)
                return NotFound(new { mensaje = "Mantenimiento no encontrado o sin cliente asociado." });

            return Ok(new { mensaje = "Recordatorio enviado correctamente." });
        }

        // Actualizar estado del servicio
        public class ActualizarEstadoRequest
        {
            public int IdEstado { get; set; }
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoRequest body)
        {
            var (ok, mensaje) = await _mantenimientosService.ActualizarEstadoAsync(id, body.IdEstado);

            if (!ok)
            {
                if (mensaje == "Mantenimiento no encontrado")
                    return NotFound(new { mensaje });

                return StatusCode(500, new { mensaje });
            }

            return Ok(new { mensaje });
        }
    }
}
