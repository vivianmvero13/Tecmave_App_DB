using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class RevisionService
    {

        private readonly AppDbContext _context;

        public RevisionService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<RevisionModel> _canton = new List<RevisionModel>();
        private int _nextId = 1;


        //funcion de obtener cantons
        public List<RevisionModel> GetRevisionModel()
        { 
                return _context.revision.ToList(); 
        }


        public RevisionModel GetById(int id) {
            return _context.revision.FirstOrDefault(p=> p.id_servicio == id);
        }

        public RevisionModel AddRevision(RevisionModel RevisionModel)
        {
            _context.revision.Add(RevisionModel);
            _context.SaveChanges();
            return RevisionModel;
        }


        public bool UpdateRevision(RevisionModel RevisionModel)
        {
            var entidad =  _context.revision.FirstOrDefault(p => p.id_servicio == RevisionModel.id_servicio);

            if (entidad == null) {
                return false;
            }

            _context.SaveChanges();

            return true;

        }


        public bool DeleteRevision(int id)
        {
            var entidad = _context.revision.FirstOrDefault(p => p.id_servicio == id);

            if (entidad == null)
            {
                return false;
            }

            _context.revision.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
