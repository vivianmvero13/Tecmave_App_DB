using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Models.Dto;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevisionController : Controller
    {
        private readonly RevisionService _RevisionService;
        private readonly AppDbContext _context;
        public RevisionController(RevisionService RevisionService, AppDbContext context)
        {
            _RevisionService = RevisionService;
            _context= context;
           
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<RevisionModel>> GetRevisionModel()
        {
            return _RevisionService.GetRevisionModel();
        }

        [HttpGet("{id}")]
        public ActionResult<RevisionModel> GetById(int id)
        {
            return _RevisionService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<RevisionModel> AddRevision(RevisionModel RevisionModel)
        {

            var newRevisionModel = _RevisionService.AddRevision(RevisionModel);

            return
                CreatedAtAction(
                        nameof(GetRevisionModel), new
                        {
                            id = newRevisionModel.id_revision,
                        },
                        newRevisionModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateRevision(RevisionModel RevisionModel)
        {

            if (!_RevisionService.UpdateRevision(RevisionModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de servicio no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        [HttpPost("finalizar-proforma")]
        public IActionResult FinalizarProforma([FromBody] FinalizarProformaDto dto)
        {
            if (dto == null || dto.id_revision <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "Datos inválidos para finalizar la proforma"
                });
            }

            var resultado = _RevisionService.FinalizarProforma(dto);

            if (!resultado)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró la revisión a finalizar"
                });
            }

            return Ok(new
            {
                mensaje = "Proforma finalizada correctamente"
            });
        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteRevisionModel(int id)
        {

            if (!_RevisionService.DeleteRevision(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de servicio no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        [HttpGet("Detalle/{id}")]
        public ActionResult GetDetalle(int id)
        {
            var rev = _RevisionService.GetById(id);
            if (rev == null) return NotFound();

            var vehiculo = _context.Vehiculos.FirstOrDefault(v => v.IdVehiculo == rev.vehiculo_id);
            var ag = _context.agendamientos.FirstOrDefault(a => a.id_agendamiento == rev.id_agendamiento);

            var servRev = _context.servicios_revision.FirstOrDefault(x => x.revision_id == id);
            var servicio = servRev != null ? _context.servicios.FirstOrDefault(s => s.id_servicio == servRev.servicio_id) : null;
            var tipoServicio = servicio != null ? _context.tipo_servicios.FirstOrDefault(t => t.id_tipo_servicio == servicio.tipo_servicio_id) : null;

            var pertenencias = _context.revision_pertenencias.Where(p => p.revision_id == id).ToList();
            var trabajos = _context.revision_trabajos.Where(t => t.revision_id == id).ToList();

            return Ok(new
            {
                vehiculo,
                servicio,
                tipoServicio,
                agendamiento = ag,
                pertenencias,
                trabajos
            });
        }

        [HttpGet("Completadas/{clienteId}")]
        public ActionResult GetRevisionesCompletadas(int clienteId)
        {
            // 1. Obtener todos los vehículos del cliente
            var vehiculos = _context.Vehiculos
                .Where(v => v.ClienteId == clienteId)
                .Select(v => v.IdVehiculo)
                .ToList();

            if (!vehiculos.Any())
                return Ok(new List<object>());

            // 2. Obtener revisiones completadas (id_estado = 7)
            var revisiones = _context.revision
                .Where(r => vehiculos.Contains(r.vehiculo_id) && r.id_estado == 7)
                .ToList();

            if (!revisiones.Any())
                return Ok(new List<object>());

            // 3. Hacer JOIN con servicios_revision y servicios
            var data =
                from rev in revisiones
                join srvRev in _context.servicios_revision
                    on rev.id_revision equals srvRev.revision_id
                join srv in _context.servicios
                    on srvRev.servicio_id equals srv.id_servicio
                select new
                {
                    revision_id = rev.id_revision,
                    servicio_id = srv.id_servicio,
                    servicio_nombre = srv.nombre
                };

            return Ok(data.ToList());
        }





    }
}
