using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class EstadosService
    {

        private readonly AppDbContext _context;

        public EstadosService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<EstadosModel> _canton = new List<EstadosModel>();
        private int _nextId = 1;


        //funcion de obtener cantons
        public List<EstadosModel> GetEstadosModel()
        {
            return _context.estados.ToList();
        }


        public EstadosModel GetById(int id)
        {
            return _context.estados.FirstOrDefault(p => p.id_estado == id);
        }

        public EstadosModel AddEstados(EstadosModel EstadosModel)
        {
            _context.estados.Add(EstadosModel);
            _context.SaveChanges();
            return EstadosModel;
        }


        public bool UpdateEstados(EstadosModel EstadosModel)
        {
            var entidad = _context.estados.FirstOrDefault(p => p.id_estado == EstadosModel.id_estado);

            if (entidad == null)
            {
                return false;
            }

            entidad.nombre = EstadosModel.nombre;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteEstados(int id)
        {
            var entidad = _context.estados.FirstOrDefault(p => p.id_estado == id);

            if (entidad == null)
            {
                return false;
            }

            _context.estados.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
