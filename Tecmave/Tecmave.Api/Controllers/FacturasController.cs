using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq; // <-- necesario
using System.Threading.Tasks;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("facturas")]
    public class FacturasController : ControllerBase
    {
        private readonly FacturasService _facturasService;
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _db;

        public FacturasController(FacturasService facturasService, IEmailSender emailSender, AppDbContext db)
        {
            _facturasService = facturasService;
            _emailSender = emailSender;
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _facturasService.GetAll();
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _facturasService.GetById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // ====== CAMBIO DE ESTADO (por body) ======
        [HttpPut("{id:int}/estado")]
        [HttpPatch("{id:int}/estado")]
        [HttpPost("{id:int}/estado")]
        [Consumes("application/json", "application/x-www-form-urlencoded")]
        public IActionResult UpdateEstado(int id, [FromBody] EstadoDto? dto)
        {
            string nuevo = dto?.estado ?? (Request.Query.ContainsKey("estado") ? Request.Query["estado"].ToString() : null);
            if (string.IsNullOrWhiteSpace(nuevo))
                return BadRequest(new { message = "estado requerido" });

            var norm = (nuevo ?? "").Trim().ToLower();
            if (new[] { "pagada", "pagado" }.Contains(norm)) nuevo = "Pagada";
            else if (new[] { "pendiente", "pending" }.Contains(norm)) nuevo = "Pendiente";
            else if (new[] { "anulada", "anulado", "cancelada", "cancelado" }.Contains(norm)) nuevo = "Anulada";

            var ok = _facturasService.UpdateEstado(id, nuevo);
            if (!ok) return NotFound(new { message = "Factura no encontrada" });

            return Ok(new { id_factura = id, estado = nuevo });
        }

        // ====== CAMBIO DE ESTADO (por ruta) ======
        [HttpPut("{id:int}/estado/{nuevo}")]
        [HttpPatch("{id:int}/estado/{nuevo}")]
        [HttpPost("{id:int}/estado/{nuevo}")]
        public IActionResult UpdateEstadoByRoute(int id, string nuevo)
        {
            if (string.IsNullOrWhiteSpace(nuevo))
                return BadRequest(new { message = "estado requerido" });

            var norm = (nuevo ?? "").Trim().ToLower();
            if (new[] { "pagada", "pagado" }.Contains(norm)) nuevo = "Pagada";
            else if (new[] { "pendiente", "pending" }.Contains(norm)) nuevo = "Pendiente";
            else if (new[] { "anulada", "anulado", "cancelada", "cancelado" }.Contains(norm)) nuevo = "Anulada";

            var ok = _facturasService.UpdateEstado(id, nuevo);
            if (!ok) return NotFound(new { message = "Factura no encontrada" });

            return Ok(new { id_factura = id, estado = nuevo });
        }

        // ====== DTOs LIMPIOS Y SEPARADOS ======
        public class EstadoDto
        {
            public string? estado { get; set; }
        }

        public class CreateFacturaDto
        {
            public int cliente_id { get; set; }
            public DateTime? fecha_emision { get; set; }
            public decimal total { get; set; }
            public string? metodo_pago { get; set; }
            public string? estado_pago { get; set; } = "Pendiente";
        }

        public class GenerateFacturaFromServiceDto
        {
            public int cliente_id { get; set; }
            public int servicio_id { get; set; }
            public string? metodo_pago { get; set; }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateFacturaDto dto)
        {
            if (dto == null || dto.cliente_id <= 0 || dto.total <= 0)
                return BadRequest(new { message = "Datos inválidos" });

            var created = _facturasService.Add(new FacturasModel
            {
                cliente_id = dto.cliente_id,
                fecha_emision = dto.fecha_emision ?? DateTime.UtcNow,
                total = dto.total,
                metodo_pago = dto.metodo_pago,
                estado_pago = string.IsNullOrWhiteSpace(dto.estado_pago) ? "Pendiente" : dto.estado_pago
            });
            return Created($"/facturas/{created.id_factura}", created);
        }

        [HttpPost("generar-automatico")]
        public IActionResult GenerateAutomatic([FromBody] GenerateFacturaFromServiceDto dto)
        {
            if (dto == null || dto.cliente_id <= 0 || dto.servicio_id <= 0)
                return BadRequest(new { message = "cliente_id y servicio_id son requeridos" });

            var created = _facturasService.GenerateFromService(dto.cliente_id, dto.servicio_id, dto.metodo_pago);
            if (created == null) return NotFound(new { message = "Servicio no encontrado" });
            return Created($"/facturas/{created.id_factura}", created);
        }

        [HttpPost("{id:int}/enviar-email")]
        public async Task<IActionResult> EnviarEmail(int id, [FromBody] dynamic? body)
        {
            var factura = _facturasService.GetById(id);
            if (factura == null) return NotFound(new { message = "Factura no encontrada" });

            string? email = null;
            try { email = (string?)body?.email; } catch { }

            if (string.IsNullOrWhiteSpace(email) && factura.cliente_id.HasValue)
            {
                email = await _db.Users
                    .Where(u => u.Id == factura.cliente_id.Value)
                    .Select(u => u.Email)
                    .FirstOrDefaultAsync();
            }
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { message = "No se encontró correo del cliente" });

            var subject = $"Factura #{factura.id_factura}";
            var total = factura.total?.ToString("C2") ?? "0.00";
            var fecha = factura.fecha_emision?.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
            var bodyHtml = $@"Hola,<br/><br/>
Adjuntamos los datos de su factura:<br/>
<strong>ID:</strong> {factura.id_factura}<br/>
<strong>Fecha:</strong> {fecha}<br/>
<strong>Total:</strong> {total}<br/>
<strong>Método de pago:</strong> {factura.metodo_pago}<br/>
<strong>Estado:</strong> {factura.estado_pago}<br/><br/>
Gracias por su preferencia.";

            try
            {
                await _emailSender.SendAsync(email!, subject, bodyHtml);
                return Accepted(new { message = "Factura enviada por correo", id_factura = id, to = email });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No fue posible enviar el correo (SMTP)", error = ex.Message });
            }
        }

        [HttpPost("recordatorios/pendientes")]
        public async Task<IActionResult> EnviarRecordatoriosPendientes()
        {
            var pendientes = await _db.factura
                .Where(f => f.estado_pago != null && f.estado_pago.ToLower() == "pendiente" && f.cliente_id != null)
                .ToListAsync();

            int enviados = 0, sinEmail = 0, errores = 0;
            var detalles = new System.Collections.Generic.List<object>();

            foreach (var f in pendientes)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == f.cliente_id);
                var email = user?.Email;
                if (string.IsNullOrWhiteSpace(email))
                {
                    sinEmail++;
                    detalles.Add(new { id_factura = f.id_factura, motivo = "Cliente sin correo" });
                    continue;
                }

                var subject = $"Recordatorio de pago — Factura #{f.id_factura}";
                var totalTxt = (f.total ?? 0m).ToString("N2");
                var bodyHtml = $@"<p>Hola {user?.Nombre ?? user?.UserName ?? "cliente"},</p>
<p>Se le recuerda que tiene un pago pendiente por <strong>{totalTxt}</strong> correspondiente a la factura <strong>#{f.id_factura}</strong>.</p>
<p>Si ya realizó el pago, puede ignorar este mensaje.</p>
<p>Saludos,<br/>TECMAVE</p>";

                try
                {
                    await _emailSender.SendAsync(email!, subject, bodyHtml);
                    enviados++;
                    detalles.Add(new { id_factura = f.id_factura, enviado = true, to = email });
                }
                catch (System.Exception ex)
                {
                    errores++;
                    detalles.Add(new { id_factura = f.id_factura, error = ex.Message });
                }
            }

            return Ok(new { pendientes = pendientes.Count, enviados, sin_email = sinEmail, errores, detalles });
        }

    }
}