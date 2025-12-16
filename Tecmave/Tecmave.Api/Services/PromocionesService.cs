using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace Tecmave.Api.Services
{
    public class PromocionesService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;
        private readonly NotificacionesService _notificacionesService;

        public PromocionesService(
            AppDbContext context,
            EmailService emailService,
            NotificacionesService notificacionesService)
        {
            _context = context;
            _emailService = emailService;
            _notificacionesService = notificacionesService;
        }

        public List<PromocionesModel> GetPromocionesModel() =>
            _context.promociones.ToList();

        public PromocionesModel GetByid_promocion(int id) =>
            _context.promociones.FirstOrDefault(p => p.id_promocion == id);

        public PromocionesModel AddPromociones(PromocionesModel model)
        {
            _context.promociones.Add(model);
            _context.SaveChanges();
            return model;
        }

        public bool UpdatePromociones(PromocionesModel model)
        {
            var entidad = _context.promociones.FirstOrDefault(p => p.id_promocion == model.id_promocion);
            if (entidad == null)
                return false;

            entidad.titulo = model.titulo;
            entidad.descripcion = model.descripcion;
            entidad.fecha_inicio = model.fecha_inicio;
            entidad.fecha_fin = model.fecha_fin;
            entidad.id_estado = model.id_estado;
            entidad.imagen_url = model.imagen_url;

            _context.SaveChanges();
            return true;
        }

        public bool DeletePromociones(int id)
        {
            var entidad = _context.promociones.FirstOrDefault(p => p.id_promocion == id);
            if (entidad == null)
                return false;

            _context.promociones.Remove(entidad);
            _context.SaveChanges();
            return true;
        }

        public async Task<int> EnviarPromocionMasivoPorPromoAsync(int idPromocion)
        {
            var promo = await _context.promociones
                .FirstOrDefaultAsync(p => p.id_promocion == idPromocion);

            if (promo == null)
                return -1; // no existe

            var usuarios = await _context.usuarios
                .Where(u => u.NotificacionesActivadas && !string.IsNullOrEmpty(u.Email))
                .ToListAsync();

            if (!usuarios.Any())
                return -2; // no hay suscritos

            int totalEnviadas = 0;

            string tituloSafe = WebUtility.HtmlEncode(promo.titulo ?? string.Empty);
            string descSafe = WebUtility.HtmlEncode(promo.descripcion ?? string.Empty);

            string imagenHtml = string.Empty;
            if (!string.IsNullOrWhiteSpace(promo.imagen_url))
            {
                var imgUrlSafe = WebUtility.HtmlEncode(promo.imagen_url);
                imagenHtml = $@"
            <div style=""margin: 16px 0;"">
                <img src=""{imgUrlSafe}""
                     alt=""{tituloSafe}""
                     style=""max-width:100%;height:auto;border-radius:8px;display:block;margin:0 auto;"" />
            </div>";
            }

            foreach (var usuario in usuarios)
            {
                bool yaEnviada = await _context.promocion_envios.AnyAsync(pe =>
                    pe.IdUsuario == usuario.Id && pe.IdPromocion == promo.id_promocion);

                if (yaEnviada)
                    continue;

                string asunto = $"Nueva promoción: {promo.titulo}";

                string cuerpo = $@"
<html>
  <body style=""font-family: Arial, sans-serif; font-size: 14px; color: #333;"">
    <div style=""max-width: 600px; margin: 0 auto; padding: 16px;"">
      <h2 style=""color:#ff8c00; margin-top:0;"">{tituloSafe}</h2>

      <p style=""line-height:1.5;"">
        {descSafe}
      </p>

      {imagenHtml}

      <p style=""margin-top:16px;"">
        <b>Válida desde:</b> {promo.fecha_inicio:dd/MM/yyyy}
        <br />
        <b>Válida hasta:</b> {promo.fecha_fin:dd/MM/yyyy}
      </p>

      <hr style=""margin:24px 0; border:none; border-top:1px solid #eee;"" />

      <p style=""font-size:12px; color:#777;"">
        Estás recibiendo este correo porque tenés activadas las notificaciones en Tecmave.
      </p>
    </div>
  </body>
</html>";

                await _emailService.EnviarCorreo(usuario.Email, asunto, cuerpo);

                _context.promocion_envios.Add(new PromocionEnvio
                {
                    IdUsuario = usuario.Id,
                    IdPromocion = promo.id_promocion,
                    FechaEnvio = DateTime.UtcNow
                });

                totalEnviadas++;
            }

            if (totalEnviadas == 0)
                return 0; // existía promo y usuarios, pero ya estaba enviado a todos

            await _context.SaveChangesAsync();
            return totalEnviadas;
        }

        public async Task EnviarRecordatoriosAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            var proximas = await _context.promociones
                .Where(p => p.fecha_fin == hoy.AddDays(3) && !p.recordatorio_enviado)
                .ToListAsync();

            foreach (var promo in proximas)
            {
                var usuarios = await _context.usuarios
                    .Where(u => u.NotificacionesActivadas)
                    .ToListAsync();

                foreach (var usuario in usuarios)
                {
                    string mensaje = $"La promoción '{promo.titulo}' finaliza el {promo.fecha_fin:dd/MM/yyyy}. Aproveche antes de que expire.";
                    string link = $"http://tecmave.com/promociones/{promo.id_promocion}";

                    await _notificacionesService.CrearNotificacionAsync(usuario.Id, mensaje, "promocion");
                    Console.WriteLine($"[Push] Enviado a {usuario.Email}: {mensaje} (Link: {link})");
                }

                promo.recordatorio_enviado = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
