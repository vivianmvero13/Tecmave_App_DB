using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class RevisionTrabajosService
    {
       
       

            private readonly AppDbContext _context;

            public RevisionTrabajosService(AppDbContext context)
            {
                _context = context;
            }

            //Aca necesitamos el modelo de datos para el almacenamiento temporal
            private readonly List<RevisionTrabajosModel> _canton = new List<RevisionTrabajosModel>();
            private int _nextId = 1;


            //funcion de obtener cantons
            public List<RevisionTrabajosModel> GetRevisionTrabajoModel()
            {
                return _context.revision_trabajos.ToList();
            }


        public List<RevisionTrabajosModel> GetTrabajosByRevisionId(int revisionId)
        {
            return _context.revision_trabajos
                .Where(p => p.revision_id == revisionId)
                .ToList();
        }

        public RevisionTrabajosModel GetById(int id)
            {
                return _context.revision_trabajos.FirstOrDefault(p => p.id_trabajo == id);
            }

            public RevisionTrabajosModel AddRevisionTrabajo(RevisionTrabajosModel RevisionTrabajosModel)
            {
                _context.revision_trabajos.Add(RevisionTrabajosModel);
                _context.SaveChanges();
                return RevisionTrabajosModel;
            }

        public bool DeletePorRevisionYNombre(int revisionId, string nombre)
        {
            var entidad = _context.revision_trabajos
                .FirstOrDefault(p => p.revision_id == revisionId && p.nombre == nombre);
            if (entidad == null)
            {
                return false;
            }
            _context.revision_trabajos.Remove(entidad);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateRevisionTrabajo(RevisionTrabajosModel RevisionDiagnosticoModel)
            {
                var entidad = _context.revision_trabajos.FirstOrDefault(p => p.id_trabajo == RevisionDiagnosticoModel.id_trabajo);

                if (entidad == null)
                {
                    return false;
                }

                _context.SaveChanges();

                return true;

            }


            public bool DeleteRevisionTrabajo(int id)
            {
                var entidad = _context.revision_trabajos.FirstOrDefault(p => p.id_trabajo == id);

                if (entidad == null)
                {
                    return false;
                }

                _context.revision_trabajos.Remove(entidad);
                _context.SaveChanges();
                return true;

            }

        }
    }
