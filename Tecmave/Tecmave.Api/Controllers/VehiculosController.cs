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
            // Verificamos que exista el vehículo
            var existe = await _db.Vehiculos.AnyAsync(x => x.IdVehiculo == id);
            if (!existe)
                return NotFound(new { message = $"Vehículo {id} no existe." });

            using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                // IMPORTANTES:
                // - Nombres de tabla/cólumnas igual que en MySQL: agendamiento, revision, etc.
                // - El orden respeta las FKs:
                //   hijos de revision -> revision -> agendamiento -> otros -> vehiculos

                var idVehiculo = id;

                // 1) Hijos de revision (servicios_revision, revision_pertenencias, revision_trabajos, resenas)

                // 1.1) servicios_revision
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM servicios_revision
            WHERE revision_id IN (
                SELECT id_revision
                FROM revision
                WHERE vehiculo_id = {idVehiculo}
                   OR id_agendamiento IN (
                       SELECT id_agendamiento
                       FROM agendamiento
                       WHERE vehiculo_id = {idVehiculo}
                   )
            );
        ");

                // 1.2) revision_pertenencias
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM revision_pertenencias
            WHERE revision_id IN (
                SELECT id_revision
                FROM revision
                WHERE vehiculo_id = {idVehiculo}
                   OR id_agendamiento IN (
                       SELECT id_agendamiento
                       FROM agendamiento
                       WHERE vehiculo_id = {idVehiculo}
                   )
            );
        ");

                // 1.3) revision_trabajos
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM revision_trabajos
            WHERE revision_id IN (
                SELECT id_revision
                FROM revision
                WHERE vehiculo_id = {idVehiculo}
                   OR id_agendamiento IN (
                       SELECT id_agendamiento
                       FROM agendamiento
                       WHERE vehiculo_id = {idVehiculo}
                   )
            );
        ");

                // 1.4) resenas
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM resenas
            WHERE revision_id IN (
                SELECT id_revision
                FROM revision
                WHERE vehiculo_id = {idVehiculo}
                   OR id_agendamiento IN (
                       SELECT id_agendamiento
                       FROM agendamiento
                       WHERE vehiculo_id = {idVehiculo}
                   )
            );
        ");

                // 2) Revisiones del vehículo (por vehiculo_id o por agendamiento del vehículo)
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM revision
            WHERE vehiculo_id = {idVehiculo}
               OR id_agendamiento IN (
                   SELECT id_agendamiento
                   FROM agendamiento
                   WHERE vehiculo_id = {idVehiculo}
               );
        ");

                // 3) Agendamientos del vehículo
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM agendamiento
            WHERE vehiculo_id = {idVehiculo};
        ");

                // 4) Recordatorios ligados al vehículo (no tienen FK, pero los limpiamos)
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM recordatorios
            WHERE VehiculoId = {idVehiculo};
        ");

                // 5) Mantenimientos del vehículo (además de tener ON DELETE CASCADE)
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM mantenimientos
            WHERE IdVehiculo = {idVehiculo};
        ");

                // 6) Finalmente, el vehículo
                await _db.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM vehiculos
            WHERE id_vehiculo = {idVehiculo};
        ");

                await tx.CommitAsync();

                _log.LogInformation("Vehículo {Id} y sus registros asociados fueron eliminados correctamente.", id);
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