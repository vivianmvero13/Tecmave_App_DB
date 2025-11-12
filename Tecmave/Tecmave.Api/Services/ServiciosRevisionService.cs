using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class ServiciosRevisionService
    {

        private readonly AppDbContext _context;

        public ServiciosRevisionService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<ServiciosRevisionModel> _canton = new List<ServiciosRevisionModel>();
        private int _nextid_servicios_revision = 1;


        //funcion de obtener cantons
        public List<ServiciosRevisionModel> GetServiciosRevisionModel()
        {
            return _context.servicios_revision.ToList();
        }


        public ServiciosRevisionModel GetByid_servicios_revision(int id)
        {
            return _context.servicios_revision.FirstOrDefault(p => p.id_servicio_revision == id);
        }

        public ServiciosRevisionModel AddServiciosRevision(ServiciosRevisionModel ServiciosRevisionModel)
        {
            _context.servicios_revision.Add(ServiciosRevisionModel);
            _context.SaveChanges();
            return ServiciosRevisionModel;
        }


        public bool UpdateServiciosRevision(ServiciosRevisionModel ServiciosRevisionModel)
        {
            var entidad = _context.servicios_revision.FirstOrDefault(p => p.id_servicio_revision == ServiciosRevisionModel.id_servicio_revision);

            if (entidad == null)
            {
                return false;
            }

            entidad.costo_final = ServiciosRevisionModel.costo_final;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteServiciosRevision(int id)
        {
            var entidad = _context.servicios_revision.FirstOrDefault(p => p.id_servicio_revision == id);

            if (entidad == null)
            {
                return false;
            }

            _context.servicios_revision.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
