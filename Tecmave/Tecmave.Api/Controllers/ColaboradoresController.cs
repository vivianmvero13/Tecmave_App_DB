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
        public ColaboradoresController(ColaboradoresService ColaboradoresService, UserAdminService userService)
        {
            _ColaboradoresService = ColaboradoresService;
            _userService = userService;
        }

        [HttpGet("buscar")]
        public ActionResult<IEnumerable<object>> BuscarColaboradores([FromQuery] string? nombre, [FromQuery] int? estado)
        {
            var colaboradores = _ColaboradoresService.GetColaboradoresModel();

            if(!string.IsNullOrEmpty(nombre))
            {
                colaboradores = colaboradores
                    .Where(c =>
                    {
                        var usuario = _userService.GetByIdAsync(c.id_usuario).Result;
                        return usuario != null && !string.IsNullOrEmpty(usuario.Nombre) &&
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

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<ColaboradoresModel>> GetColaboradoresModel()
        {
            return _ColaboradoresService.GetColaboradoresModel();
        }

        [HttpGet("{id}")]
        public ActionResult<ColaboradoresModel> GetById(int id)
        {
            return _ColaboradoresService.GetByid_colaborador(id);
        }

        //Apis POST
        [HttpPost]
        public async Task<ActionResult<ColaboradoresModel>> AddColaboradores([FromBody] Colaborador dto)
        {

            var newColaboradoresModel = await _ColaboradoresService.AddColaboradoresAsync(dto);

            return
                CreatedAtAction(
                    nameof(GetById),
                    new { id = newColaboradoresModel.id_colaborador},
                    newColaboradoresModel
                );
            
        }

        //APIS PUT
        [HttpPut("editar-completo")]
        public async Task<IActionResult> UpdateColaboradores([FromBody] EditarColaboradorDto dto)
        {

            bool actualizado = await _ColaboradoresService.UpdateColaboradorAsync(dto);

            if (!actualizado)
            {
                return NotFound(new { mensaje = "El colaborador no se encontró en el sistema" });
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColaboradoresModel(int id)
        {

            bool eliminado = await _ColaboradoresService.DeleteColaboradoresAsync(id);

            if (!eliminado)
            {
                return NotFound(new { mensaje = "El colaborador no fue encontrado en el sistema " });
            }

            return NoContent();

        }

    }
}
