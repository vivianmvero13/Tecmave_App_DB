using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class TipoServiciosService
    {

        private readonly AppDbContext _context;

        public TipoServiciosService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<TipoServiciosModel> _canton = new List<TipoServiciosModel>();
        private int _nextId = 1;


        //funcion de obtener cantons
        public List<TipoServiciosModel> GetTipoServiciosModel()
        {
            return _context.tipo_servicios.ToList();
        }


        public TipoServiciosModel GetById(int id)
        {
            return _context.tipo_servicios.FirstOrDefault(p => p.id_tipo_servicio == id);
        }

        public TipoServiciosModel AddTipoServicios(TipoServiciosModel TipoServiciosModel)
        {
            _context.tipo_servicios.Add(TipoServiciosModel);
            _context.SaveChanges();
            return TipoServiciosModel;
        }


        public bool UpdateTipoServicios(TipoServiciosModel TipoServiciosModel)
        {
            var entidad = _context.tipo_servicios.FirstOrDefault(p => p.id_tipo_servicio == TipoServiciosModel.id_tipo_servicio);

            if (entidad == null)
            {
                return false;
            }

            entidad.nombre = TipoServiciosModel.nombre;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteTipoServicios(int id)
        {
            var entidad = _context.tipo_servicios.FirstOrDefault(p => p.id_tipo_servicio == id);

            if (entidad == null)
            {
                return false;
            }

            _context.tipo_servicios.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
