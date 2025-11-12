using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("vehiculos")]
    public class VehiculosController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<VehiculosController> _log;

        public VehiculosController(AppDbContext db, ILogger<VehiculosController> log)
        {
            _db = db; _log = log;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Vehiculo>), 200)]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> List()
            => Ok(await _db.Vehiculos.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Vehiculo), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Vehiculo>> GetById(int id)
            => await _db.Vehiculos.FindAsync(id) is { } v ? Ok(v) : NotFound();

        [HttpPost]
        [ProducesResponseType(typeof(Vehiculo), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Create([FromBody] Vehiculo dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (dto.Anno < 1950 || dto.Anno > DateTime.UtcNow.Year + 1)
                return BadRequest(new { message = "Año fuera de rango." });

            var v = new Vehiculo
            {
                ClienteId = dto.ClienteId,
                IdMarca = dto.IdMarca,
                Anno = dto.Anno,
                Placa = (dto.Placa ?? string.Empty).ToUpperInvariant()
            };

            _db.Vehiculos.Add(v);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _log.LogWarning(ex, "Error creando vehículo (posible placa duplicada).");
                return Conflict(new { message = "No se pudo crear el vehículo (posible duplicado de placa)." });
            }

            return CreatedAtAction(nameof(GetById), new { id = v.IdVehiculo }, v);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Update([FromBody] Vehiculo dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var v = await _db.Vehiculos.FirstOrDefaultAsync(x => x.IdVehiculo == dto.IdVehiculo);
            if (v is null)
                return NotFound(new { message = $"Vehículo {dto.IdVehiculo} no existe." });

            if (dto.Anno < 1950 || dto.Anno > DateTime.UtcNow.Year + 1)
                return BadRequest(new { message = "Año fuera de rango." });

            v.ClienteId = dto.ClienteId;
            v.IdMarca = dto.IdMarca;
            v.Anno = dto.Anno;
            v.Placa = (dto.Placa ?? string.Empty).ToUpperInvariant();

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _log.LogWarning(ex, "Error actualizando vehículo (posible placa duplicada).");
                return Conflict(new { message = "No se pudo actualizar el vehículo (posible duplicado de placa)." });
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var v = await _db.Vehiculos.FirstOrDefaultAsync(x => x.IdVehiculo == id);
            if (v is null)
                return NotFound(new { message = $"Vehículo {id} no existe." });

            _db.Vehiculos.Remove(v);
            await _db.SaveChangesAsync();

            _log.LogInformation("Vehículo {Id} eliminado correctamente.", id);
            return NoContent();
        }
    }
}