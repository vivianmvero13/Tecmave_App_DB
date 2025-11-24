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


        public bool UpdateRevisionPertenencia(RevisionPertenenciasModel RevisionDiagnosticoModel)
        {
            var entidad = _context.revision_pertenencias.FirstOrDefault(p => p.id_pertenencia == RevisionDiagnosticoModel.id_pertenencia);

            if (entidad == null)
            {
                return false;
            }

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
