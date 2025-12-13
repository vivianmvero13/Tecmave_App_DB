using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class RevisionPertenenciasService
    {

        private readonly AppDbContext _context;

        public RevisionPertenenciasService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<RevisionPertenenciasModel> _canton = new List<RevisionPertenenciasModel>();
        private int _nextId = 1;


        //funcion de obtener cantons
        public List<RevisionPertenenciasModel> GetRevisionPertenenciaModel()
        {
            return _context.revision_pertenencias.ToList();
        }

        public List<RevisionPertenenciasModel> GetPertenenciasByRevisionId(int revisionId)
        {
            return _context.revision_pertenencias
                .Where(p => p.revision_id == revisionId)
                .ToList();
        }

        public RevisionPertenenciasModel GetById(int id)
        {
            return _context.revision_pertenencias.FirstOrDefault(p => p.id_pertenencia == id);
        }

        public RevisionPertenenciasModel AddRevisionPertenencia(RevisionPertenenciasModel RevisionPertenenciasModel)
        {
            _context.revision_pertenencias.Add(RevisionPertenenciasModel);
            _context.SaveChanges();
            return RevisionPertenenciasModel;
        }


        public bool DeletePorRevisionYNombre(int revisionId, string nombre)
        {
            var entidad = _context.revision_pertenencias
                .FirstOrDefault(p => p.revision_id == revisionId && p.nombre == nombre);
            if (entidad == null)
            {
                return false;
            }
            _context.revision_pertenencias.Remove(entidad);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateRevisionPertenencia(RevisionPertenenciasModel RevisionDiagnosticoModel)
        {
            var entidad = _context.revision_pertenencias.FirstOrDefault(p => p.id_pertenencia == RevisionDiagnosticoModel.id_pertenencia);

            if (entidad == null)
            {
                return false;
            }

            entidad.nombre = RevisionDiagnosticoModel.nombre;
            
            _context.SaveChanges();

            return true;

        }


        public bool DeleteRevisionPertenencia(int id)
        {
            var entidad = _context.revision_pertenencias.FirstOrDefault(p => p.id_pertenencia == id);

            if (entidad == null)
            {
                return false;
            }

            _context.revision_pertenencias.Remove(entidad);
            _context.SaveChanges();
            return true;

        }

    }
}
