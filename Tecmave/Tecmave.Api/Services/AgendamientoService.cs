using System.Collections.Generic;
using System.Linq;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Models.Dto;

namespace Tecmave.Api.Services
{
    public class AgendamientoService
    {
        private readonly AppDbContext _context;

        public AgendamientoService(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos
        public List<AgendamientoModel> GetAgendamientoModel()
        {
            return _context.agendamientos.ToList();
        }

        // Obtener por ID
        public AgendamientoModel GetById(int id)
        {
            return _context.agendamientos.FirstOrDefault(p => p.id_agendamiento == id);
        }

        // Insertar
        public AgendamientoModel AddAgendamiento(AgendamientoModel agendamientoModel)
        {
            _context.agendamientos.Add(agendamientoModel);
            _context.SaveChanges();
            return agendamientoModel;
        }

        // Actualizar
        public bool UpdateAgendamiento(AgendamientoUpdateDto dto)
        {
            var entidad = _context.agendamientos
                .FirstOrDefault(p => p.id_agendamiento == dto.id_agendamiento);

            if (entidad == null)
            {
                return false;
            }


            entidad.vehiculo_id = dto.vehiculo_id;
            entidad.fecha_estimada = dto.fecha_estimada;
            entidad.hora_llegada = dto.hora_llegada;




            _context.SaveChanges();
            return true;
        }

        // Eliminar
        public bool DeleteAgendamiento(int id)
        {
            var entidad = _context.agendamientos.FirstOrDefault(p => p.id_agendamiento == id);

            if (entidad == null)
            {
                return false;
            }

            _context.agendamientos.Remove(entidad);
            _context.SaveChanges();
            return true;
        }
    }
}
