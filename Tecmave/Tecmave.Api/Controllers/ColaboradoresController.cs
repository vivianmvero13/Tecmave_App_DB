using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Models.Dto;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColaboradoresController : Controller
    {
        private readonly ColaboradoresService _ColaboradoresService;
        private readonly UserAdminService _userService;

        public ColaboradoresController(
            ColaboradoresService ColaboradoresService,
            UserAdminService userService)
        {
            _ColaboradoresService = ColaboradoresService;
            _userService = userService;
        }

      
        [HttpGet("buscar")]
        public ActionResult<IEnumerable<object>> BuscarColaboradores(
            [FromQuery] string? nombre,
            [FromQuery] int? estado)
        {
            var colaboradores = _ColaboradoresService.GetColaboradoresModel();

            if (!string.IsNullOrEmpty(nombre))
            {
                colaboradores = colaboradores
                    .Where(c =>
                    {
                        var usuario = _userService.GetByIdAsync(c.id_usuario).Result;
                        return usuario != null &&
                               usuario.Nombre != null &&
                               usuario.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase);
                    })
                    .ToList();
            }

            if (estado.HasValue)
            {
                colaboradores = colaboradores
                    .Where(c =>
                    {
                        var usuario = _userService.GetByIdAsync(c.id_usuario).Result;
                        return usuario != null && usuario.Estado == estado.Value;
                    })
                    .ToList();
            }

            var resultado = colaboradores.Select(c =>
            {
                var u = _userService.GetByIdAsync(c.id_usuario).Result;

                return new
                {
                    c.id_colaborador,
                    Nombre = u != null ? $"{u.Nombre} {u.Apellido}" : "Desconocido",
                    c.puesto,
                    c.salario,
                    c.fecha_contratacion,
                    Estado = u?.Estado,
                    Rol = u != null ? string.Join(", ", _userService.GetRolesAsync(u.Id).Result) : "Sin rol"
                };
            });

            return Ok(resultado);
        }

        // ───────────────────────────────────────────────
        // GET GENERAL
        // ───────────────────────────────────────────────

        [HttpGet]
        public ActionResult<IEnumerable<ColaboradoresModel>> GetColaboradoresModel()
        {
            return _ColaboradoresService.GetColaboradoresModel();
        }

        // ───────────────────────────────────────────────
        // GET POR ID
        // ───────────────────────────────────────────────

        [HttpGet("{id}")]
        public ActionResult<ColaboradoresModel> GetById(int id)
        {
            var col = _ColaboradoresService.GetByid_colaborador(id);

            if (col == null)
                return NotFound();

            return Ok(col);
        }

        // ───────────────────────────────────────────────
        // POST
        // ───────────────────────────────────────────────

        [HttpPost]
        public async Task<ActionResult<ColaboradoresModel>> AddColaboradores([FromBody] Colaborador dto)
        {
            var newColaboradoresModel = await _ColaboradoresService.AddColaboradoresAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newColaboradoresModel.id_colaborador },
                newColaboradoresModel
            );
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColaboradores(
            int id,
            [FromBody] ColaboradoresModel model)
        {
            model.id_colaborador = id; // Asegurar ID correcto

            var actualizado = await _ColaboradoresService.UpdateColaboradoresAsync(model);

            if (!actualizado)
                return NotFound(new { mensaje = "El colaborador no se encontró en el sistema" });

            return NoContent();
        }


        // ───────────────────────────────────────────────
        // DELETE
        // ───────────────────────────────────────────────

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColaboradoresModel(int id)
        {
            var eliminado = await _ColaboradoresService.DeleteColaboradoresAsync(id);

            if (!eliminado)
                return NotFound(new { mensaje = "El colaborador no fue encontrado en el sistema" });

            return NoContent();
        }
    }
}
