using System.Runtime.CompilerServices;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class NotificacionesService
    {

        private readonly AppDbContext _context;

        public NotificacionesService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<NotificacionesModel> _canton = new List<NotificacionesModel>();
        private int _nextid_notificacion = 1;


        //funcion de obtener cantons
        public List<NotificacionesModel> GetNotificacionesModel()
        {
            return _context.notificaciones.ToList();
        }


        public NotificacionesModel GetByid_notificacion(int id)
        {
            return _context.notificaciones.FirstOrDefault(p => p.id_notificaciones == id);
        }

        public NotificacionesModel AddNotificaciones(NotificacionesModel NotificacionesModel)
        {
            _context.notificaciones.Add(NotificacionesModel);
            _context.SaveChanges();
            return NotificacionesModel;
        }


        public bool UpdateNotificaciones(NotificacionesModel NotificacionesModel)
        {
            var entidad = _context.notificaciones.FirstOrDefault(p => p.id_notificaciones == NotificacionesModel.id_notificaciones);

            if (entidad == null)
            {
                return false;
            }

            entidad.mensaje = NotificacionesModel.mensaje;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteNotificaciones(int id)
        {
            var entidad = _context.notificaciones.FirstOrDefault(p => p.id_notificaciones == id);

            if (entidad == null)
            {
                return false;
            }

            _context.notificaciones.Remove(entidad);
            _context.SaveChanges();
            return true;

        }

        public async Task CrearNotificacionAsync(int usuarioId, string mensaje, string tipo)
        {
            var notificacion = new NotificacionesModel
            {
                usuario_id = usuarioId,
                mensaje = mensaje,
                tipo = tipo,
                fecha_envio = DateOnly.FromDateTime(DateTime.Now),
                id_estado = 1
            };

            _context.notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();
        }


    }
}
