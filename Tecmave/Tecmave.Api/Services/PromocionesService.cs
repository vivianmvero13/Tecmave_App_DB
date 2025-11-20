using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            var promo = await _context.promociones
                .FirstOrDefaultAsync(p =>
                    p.id_promocion == idPromocion &&
                    p.fecha_inicio <= hoy &&
                    p.fecha_fin >= hoy);

            if (promo == null)
                return 0;

            var usuarios = await _context.usuarios
                .Where(u => u.NotificacionesActivadas && !string.IsNullOrEmpty(u.Email))
                .ToListAsync();

            if (!usuarios.Any())
                return 0;

            int totalEnviadas = 0;

            foreach (var usuario in usuarios)
            {
                bool yaEnviada = await _context.promocion_envios.AnyAsync(pe =>
                    pe.IdUsuario == usuario.Id && pe.IdPromocion == promo.id_promocion);

                if (yaEnviada)
                    continue;

                string asunto = $"Nueva promoción: {promo.titulo}";
                string cuerpo = $@"
                    <h3>{promo.titulo}</h3>
                    <p>{promo.descripcion}</p>
                    <p><b>Válida hasta:</b> {promo.fecha_fin:dd/MM/yyyy}</p>";

                await _emailService.EnviarCorreo(usuario.Email, asunto, cuerpo);

                _context.promocion_envios.Add(new PromocionEnvio
                {
                    IdUsuario = usuario.Id,
                    IdPromocion = promo.id_promocion,
                    FechaEnvio = DateTime.UtcNow
                });

                totalEnviadas++;
            }

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
