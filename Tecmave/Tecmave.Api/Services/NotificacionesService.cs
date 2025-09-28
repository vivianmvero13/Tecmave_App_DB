using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

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
            return _context.Notificaciones.ToList();
        }


        public NotificacionesModel GetByid_notificacion(int id)
        {
            return _context.Notificaciones.FirstOrDefault(p => p.id_notificaciones == id);
        }

        public NotificacionesModel AddNotificaciones(NotificacionesModel NotificacionesModel)
        {
            _context.Notificaciones.Add(NotificacionesModel);
            _context.SaveChanges();
            return NotificacionesModel;
        }


        public bool UpdateNotificaciones(NotificacionesModel NotificacionesModel)
        {
            var entidad = _context.Notificaciones.FirstOrDefault(p => p.id_notificaciones == NotificacionesModel.id_notificaciones);

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
            var entidad = _context.Notificaciones.FirstOrDefault(p => p.id_notificaciones == id);

            if (entidad == null)
            {
                return false;
            }

            _context.Notificaciones.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
