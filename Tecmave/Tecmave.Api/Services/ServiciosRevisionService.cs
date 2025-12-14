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


        public bool UpdateServiciosRevision(ServiciosRevisionModel model)
        {
            var entidad = _context.servicios_revision
                .FirstOrDefault(x => x.revision_id == model.revision_id);

            if (entidad == null)
                return false;

            entidad.servicio_id = model.servicio_id;
            entidad.costo_final = model.costo_final;

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

        public ServiciosRevisionModel? GetByRevisionId(int revisionId)
        {
            return _context.servicios_revision
                .FirstOrDefault(x => x.revision_id == revisionId);
        }


    }
}
