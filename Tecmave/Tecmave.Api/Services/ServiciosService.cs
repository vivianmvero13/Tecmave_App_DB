using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class ServiciosService
    {

        private readonly AppDbContext _context;

        public ServiciosService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<ServiciosModel> _canton = new List<ServiciosModel>();
        private int _nextId = 1;


        //funcion de obtener cantons
        public List<ServiciosModel> GetServiciosModel()
        {
            return _context.servicios.ToList();
        }


        public ServiciosModel GetById(int id)
        {
            return _context.servicios.FirstOrDefault(p => p.id_servicio == id);
        }

        public ServiciosModel AddServicios(ServiciosModel ServiciosModel)
        {
            _context.servicios.Add(ServiciosModel);
            _context.SaveChanges();
            return ServiciosModel;
        }


        public bool UpdateServicios(ServiciosModel ServiciosModel)
        {
            var entidad = _context.servicios.FirstOrDefault(p => p.id_servicio == ServiciosModel.id_servicio);

            if (entidad == null)
            {
                return false;
            }

            entidad.nombre = ServiciosModel.nombre;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteServicios(int id)
        {
            var entidad = _context.servicios.FirstOrDefault(p => p.id_servicio == id);

            if (entidad == null)
            {
                return false;
            }

            _context.servicios.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
