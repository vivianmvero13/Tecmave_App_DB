using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class PromocionesService
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;
        private readonly NotificacionesService _notificacionesService;

        public PromocionesService(AppDbContext context, EmailService emailService, NotificacionesService notificacionesService)
        {
            _context = context;
            _emailService = emailService;
            _notificacionesService = notificacionesService;
        }

        public List<PromocionesModel> GetPromocionesModel()
        {
            return _context.promociones.ToList();
        }

        public PromocionesModel GetByid_promocion(int id)
        {
            return _context.promociones.FirstOrDefault(p => p.id_promocion == id);
        }

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

        public async Task<int> EnviarPromocion(int idUsuario, string emailDestino)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            var promocionesActivas = await _context.promociones
                .Where(p => p.fecha_inicio <= hoy && p.fecha_fin >= hoy)
                .ToListAsync();

            int enviadas = 0;

            foreach (var promo in promocionesActivas)
            {
                bool yaEnviada = await _context.promocion_envios.AnyAsync(pe =>
                    pe.IdUsuario == idUsuario && pe.IdPromocion == promo.id_promocion);

                if (yaEnviada)
                    continue;

                string asunto = $"Nueva promoción: {promo.titulo}";
                string cuerpo = $@"
                    <h3>{promo.titulo}</h3>
                    <p>{promo.descripcion}</p>
                    <p><b>Válida hasta:</b> {promo.fecha_fin}</p>";

                await _emailService.EnviarCorreo(emailDestino, asunto, cuerpo);

                var registro = new PromocionEnvio
                {
                    IdUsuario = idUsuario,
                    IdPromocion = promo.id_promocion,
                    FechaEnvio = DateTime.UtcNow
                };

                _context.promocion_envios.Add(registro);
                promo.recordatorio_enviado = true;
                enviadas++;
            }

            await _context.SaveChangesAsync();
            return enviadas;
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
                    string mensaje = $"La promoción '{promo.titulo}' finaliza el {promo.fecha_fin}. ¡Aprovecha antes de que expire!";
                    string link = $"https://tecmave.com/promociones/{promo.id_promocion}";

                    await _notificacionesService.CrearNotificacionAsync(usuario.Id, mensaje, "promocion");

                    Console.WriteLine($"[Push] Enviado a {usuario.Email}: {mensaje} (Link: {link})");
                }

                promo.recordatorio_enviado = true;
            }

            await _context.SaveChangesAsync();

        }

        public bool DeletePromociones(int id)
        {
            var entidad = _context.promociones.FirstOrDefault(p => p.id_promocion == id);

            if (entidad == null)
            {
                return false;
            }

            _context.promociones.Remove(entidad);
            _context.SaveChanges();
            return true;
        }
    }
}
