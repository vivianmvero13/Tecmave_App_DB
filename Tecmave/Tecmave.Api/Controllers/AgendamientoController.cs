using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Models.Dto;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgendamientoController : Controller
    {
        private readonly AgendamientoService _agendamientoService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<Usuario> _userManager;

        public AgendamientoController(
            AgendamientoService agendamientoService,
            IEmailSender emailSender,
            UserManager<Usuario> userManager)
        {
            _agendamientoService = agendamientoService;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        // ========= GET =========
        [HttpGet]
        public ActionResult<IEnumerable<AgendamientoModel>> GetAgendamientoModel()
        {
            return _agendamientoService.GetAgendamientoModel();
        }

        [HttpGet("{id}")]
        public ActionResult<AgendamientoModel> GetById(int id)
        {
            var ag = _agendamientoService.GetById(id);
            if (ag == null) return NotFound();
            return ag;
        }

        // ========= POST =========
        [HttpPost]
        public async Task<ActionResult<AgendamientoModel>> AddAgendamiento(AgendamientoModel agendamientoModel)
        {
            var newAg = _agendamientoService.AddAgendamiento(agendamientoModel);

            // OBTENER EMAIL DESDE aspnetusers
            var user = await _userManager.FindByIdAsync(newAg.cliente_id.ToString());

            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                string subject = "Confirmación de Agendamiento - Tecmave";

                string bodyHtml = $@"
                    <h2 style='color:#0053ff'>Tu revisión ha sido agendada</h2>
                    <p>Hola <strong>{user.Nombre}</strong>,</p>
                    <p>Tu cita ha sido registrada con éxito.</p>

                    <h3>Detalles del Agendamiento:</h3>
                    <ul>
                        <li><strong>Fecha:</strong> {newAg.fecha_estimada}</li>
                        <li><strong>Hora:</strong> {newAg.hora_llegada}</li>
                    </ul>

                    <p>Gracias por confiar en Tecmave.</p>
                    <p><em>Este es un mensaje automático, por favor no responder.</em></p>
                ";

                await _emailSender.SendAsync(user.Email, subject, bodyHtml);
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = newAg.id_agendamiento },
                newAg
            );
        }

        // ========= PUT =========
        [HttpPut]
        public IActionResult UpdateAgendamiento([FromBody] AgendamientoUpdateDto dto)
        {
            if (!_agendamientoService.UpdateAgendamiento(dto))
            {
                return NotFound(new
                {
                    mensaje = "El agendamiento no fue encontrado"
                });
            }

            return NoContent();
        }

        [HttpPut("finalizar")]
        public IActionResult Finalizar([FromBody] AgendamientoFinalizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = _agendamientoService.FinalizarAgendamiento(dto);

            if (!ok)
                return NotFound();

            return Ok();
        }


        // ========= DELETE =========
        // Se mantiene así para no romper llamadas existentes: /Agendamiento?id=5
        [HttpDelete]
        public IActionResult DeleteAgendamientoModel(int id)
        {
            if (!_agendamientoService.DeleteAgendamiento(id))
            {
                return NotFound(new
                {
                    mensaje = "El agendamiento no fue encontrado"
                });
            }

            return NoContent();
        }
    }
}
