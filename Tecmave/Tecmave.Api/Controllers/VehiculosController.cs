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
            _db = db;
            _log = log;
        }

        private static string NormalizarPlaca(string? placaSinNormalizar)
        {
            return (placaSinNormalizar ?? string.Empty)
                .ToUpperInvariant()
                .Trim();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Vehiculo>), 200)]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> List()
        {
            var lista = await _db.Vehiculos
                .AsNoTracking()
                .ToListAsync();

            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Vehiculo), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Vehiculo>> GetById(int id)
        {
            var v = await _db.Vehiculos.FindAsync(id);
            if (v is null)
                return NotFound(new { message = $"Vehículo {id} no existe." });

            return Ok(v);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Vehiculo), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Create([FromBody] Vehiculo dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var year = DateTime.UtcNow.Year;
            if (dto.Anno < 1950 || dto.Anno > year + 1)
                return BadRequest(new { message = "Año fuera de rango." });

            var placaNormalizada = NormalizarPlaca(dto.Placa);

            // Validar que NO exista ya la placa
            var existePlaca = await _db.Vehiculos
                .AnyAsync(v => v.Placa == placaNormalizada);

            if (existePlaca)
            {
                return Conflict(new { message = "Ya existe un vehículo registrado con esa placa." });
            }

            var vNuevo = new Vehiculo
            {
                ClienteId = dto.ClienteId,
                IdMarca = dto.IdMarca,
                Anno = dto.Anno,
                Placa = placaNormalizada,
                Modelo = (dto.Modelo ?? string.Empty).Trim()
            };

            _db.Vehiculos.Add(vNuevo);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _log.LogWarning(ex, "Error creando vehículo (posible placa duplicada u otra restricción).");
                return Conflict(new { message = "No se pudo crear el vehículo (posible duplicado o error de base de datos)." });
            }

            return CreatedAtAction(nameof(GetById), new { id = vNuevo.IdVehiculo }, vNuevo);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Update([FromBody] Vehiculo dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var v = await _db.Vehiculos
                .FirstOrDefaultAsync(x => x.IdVehiculo == dto.IdVehiculo);

            if (v is null)
                return NotFound(new { message = $"Vehículo {dto.IdVehiculo} no existe." });

            var year = DateTime.UtcNow.Year;
            if (dto.Anno < 1950 || dto.Anno > year + 1)
                return BadRequest(new { message = "Año fuera de rango." });

            var placaNormalizada = NormalizarPlaca(dto.Placa);

            // Validar que no exista la misma placa en OTRO vehículo
            var existePlacaEnOtro = await _db.Vehiculos
                .AnyAsync(x => x.Placa == placaNormalizada && x.IdVehiculo != dto.IdVehiculo);

            if (existePlacaEnOtro)
            {
                return Conflict(new { message = "Ya existe otro vehículo registrado con esa placa." });
            }

            v.ClienteId = dto.ClienteId;
            v.IdMarca = dto.IdMarca;
            v.Anno = dto.Anno;
            v.Placa = placaNormalizada;
            v.Modelo = (dto.Modelo ?? string.Empty).Trim();

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _log.LogWarning(ex, "Error actualizando vehículo (posible placa duplicada u otra restricción).");
                return Conflict(new { message = "No se pudo actualizar el vehículo (posible duplicado o error de base de datos)." });
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            var vehiculo = await _db.Vehiculos
                .FirstOrDefaultAsync(x => x.IdVehiculo == id);

            if (vehiculo is null)
                return NotFound(new { message = $"Vehículo {id} no existe." });

            using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                // 1) Agendamientos del vehículo
                var agendamientos = await _db.agendamientos
                    .Where(a => a.vehiculo_id == id)
                    .ToListAsync();

                var agIds = agendamientos
                    .Select(a => a.id_agendamiento)
                    .ToList();

                // 2) Revisiones del vehículo:
                //    - por vehiculo_id
                //    - o por id_agendamiento dentro de los agIds
                var revisiones = await _db.revision
                    .Where(r =>
                        r.vehiculo_id == id ||
                        (agIds.Count > 0 && agIds.Contains(r.id_agendamiento)))
                    .ToListAsync();

                var revisionIds = revisiones
                    .Select(r => r.id_revision)
                    .ToList();

                if (revisionIds.Any())
                {
                    // 2.1) servicios_revision
                    var serviciosRev = await _db.servicios_revision
                        .Where(sr => revisionIds.Contains(sr.revision_id))
                        .ToListAsync();

                    if (serviciosRev.Any())
                        _db.servicios_revision.RemoveRange(serviciosRev);

                    // 2.2) revision_pertenencias
                    var pertenencias = await _db.revision_pertenencias
                        .Where(rp => revisionIds.Contains(rp.revision_id))
                        .ToListAsync();

                    if (pertenencias.Any())
                        _db.revision_pertenencias.RemoveRange(pertenencias);

                    // 2.3) revision_trabajos
                    var trabajos = await _db.revision_trabajos
                        .Where(rt => revisionIds.Contains(rt.revision_id))
                        .ToListAsync();

                    if (trabajos.Any())
                        _db.revision_trabajos.RemoveRange(trabajos);

                    // 2.4) resenas ligadas a esas revisiones
                    var resenas = await _db.resenas
                        .Where(rs => revisionIds.Contains(rs.revision_id))
                        .ToListAsync();

                    if (resenas.Any())
                        _db.resenas.RemoveRange(resenas);

                    // 2.5) finalmente, las revisiones
                    _db.revision.RemoveRange(revisiones);
                }

                // 3) Agendamientos del vehículo
                if (agendamientos.Any())
                    _db.agendamientos.RemoveRange(agendamientos);

                // 4) Recordatorios ligados al vehículo
                var recordatorios = await _db.recordatorios
                    .Where(r => r.VehiculoId == id)
                    .ToListAsync();

                if (recordatorios.Any())
                    _db.recordatorios.RemoveRange(recordatorios);

                // 5) Mantenimientos
                var mantenimientos = await _db.Mantenimientos
                    .Where(m => m.IdVehiculo == id)
                    .ToListAsync();

                if (mantenimientos.Any())
                    _db.Mantenimientos.RemoveRange(mantenimientos);

                // 6) Finalmente, el vehículo
                _db.Vehiculos.Remove(vehiculo);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                _log.LogInformation(
                    "Vehículo {Id} y sus registros asociados fueron eliminados correctamente.",
                    id);

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                await tx.RollbackAsync();
                _log.LogError(ex, "Error de base de datos al eliminar el vehículo {Id}.", id);

                return Conflict(new
                {
                    message = "No se pudo eliminar el vehículo porque aún existen registros asociados " +
                              "(por ejemplo, revisiones, agendamientos u otros que dependen del vehículo)."
                });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                _log.LogError(ex, "Error inesperado al eliminar el vehículo {Id}.", id);
                return StatusCode(500, new { message = "Error interno al eliminar el vehículo." });
            }
        }
    }
}