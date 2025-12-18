using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Models.Dto;

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


        public RevisionModel GetById(int id)
        {
            return _context.revision.FirstOrDefault(p => p.id_revision == id);
        }

        public RevisionModel AddRevision(RevisionModel RevisionModel)
        {
            _context.revision.Add(RevisionModel);
            _context.SaveChanges();
            return RevisionModel;
        }


        public bool UpdateRevision(RevisionModel RevisionModel)
        {
            var entidad = _context.revision.FirstOrDefault(p => p.id_revision == RevisionModel.id_revision);

            if (entidad == null)
            {
                return false;
            }

            
            entidad.id_estado = RevisionModel.id_estado;
            entidad.fecha_estimada_entrega = RevisionModel.fecha_estimada_entrega;
            entidad.fecha_entrega_final = RevisionModel.fecha_entrega_final;
            entidad.kilometraje = RevisionModel.kilometraje;
            entidad.nivel_combustible = RevisionModel.nivel_combustible;
            entidad.golpes_delantera = RevisionModel.golpes_delantera;
            entidad.golpes_trasera = RevisionModel.golpes_trasera;
            entidad.golpes_izquierda = RevisionModel.golpes_izquierda;
            entidad.golpes_derecha = RevisionModel.golpes_derecha;
            entidad.golpes_arriba = RevisionModel.golpes_arriba;
            entidad.vehiculo_sucio = RevisionModel.vehiculo_sucio;
            entidad.vehiculo_mojado = RevisionModel.vehiculo_mojado;
            entidad.notas_taller = RevisionModel.notas_taller;

           

            _context.SaveChanges();

            return true;
        }

        public bool FinalizarProforma(FinalizarProformaDto dto)
        {
            var entidad = _context.revision.FirstOrDefault(p => p.id_revision == dto.id_revision);
            if (entidad == null) return false;

            entidad.id_estado = dto.id_estado;
            entidad.kilometraje = dto.kilometraje;
            entidad.nivel_combustible = dto.nivel_combustible;

            entidad.golpes_delantera = dto.golpes_delantera;
            entidad.golpes_trasera = dto.golpes_trasera;
            entidad.golpes_izquierda = dto.golpes_izquierda;
            entidad.golpes_derecha = dto.golpes_derecha;
            entidad.golpes_arriba = dto.golpes_arriba;

            entidad.vehiculo_sucio = dto.vehiculo_sucio;
            entidad.vehiculo_mojado = dto.vehiculo_mojado;

            entidad.fecha_estimada_entrega = dto.fecha_estimada_entrega;
            entidad.fecha_entrega_final = dto.fecha_entrega_final;
            entidad.notas_taller = dto.notas_taller;
            entidad.id_colaborador = dto.id_colaborador;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteRevision(int id)
        {
            var entidad = _context.revision.FirstOrDefault(p => p.id_revision == id);

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
