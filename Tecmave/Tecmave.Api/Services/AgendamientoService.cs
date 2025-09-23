using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class AgendamientoService
    {

        private readonly AppDbContext _context;

        public AgendamientoService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<AgendamientoModel> _agendamiento = new List<AgendamientoModel>();
        private int _nextId = 1;


        //funcion de obtener agendamientos
        public List<AgendamientoModel> GetAgendamientoModel()
        { 
                return _context.Agendamiento.ToList(); 
        }


        public AgendamientoModel GetById(int id) {
            return _context.Agendamiento.FirstOrDefault(p=> p.id_agendamiento== id);
        }

        public AgendamientoModel AddAgendamiento(AgendamientoModel AgendamientoModel)
        {
            _context.Agendamiento.Add(AgendamientoModel);
            _context.SaveChanges();
            return AgendamientoModel;
        }


        public bool UpdateAgendamiento(AgendamientoModel AgendamientoModel)
        {
            var entidad =  _context.Agendamiento.FirstOrDefault(p => p.id_agendamiento== AgendamientoModel.id_agendamiento);

            if (entidad == null) {
                return false;
            }

            _context.SaveChanges();

            return true;

        }


        public bool DeleteAgendamiento(int id)
        {
            var entidad = _context.Agendamiento.FirstOrDefault(p => p.id_agendamiento== id);

            if (entidad == null)
            {
                return false;
            }

            _context.Agendamiento.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
