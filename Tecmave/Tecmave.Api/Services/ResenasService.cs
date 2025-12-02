using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class ResenasService
    {
        private readonly AppDbContext _context;

        public ResenasService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // CRUD BÁSICO
        // =====================================================

        public List<ResenasModel> GetResenasModel()
        {
            return _context.resenas.ToList();
        }

        public ResenasModel? GetByid_resena(int id)
        {
            return _context.resenas.FirstOrDefault(r => r.id_resena == id);
        }

        public ResenasModel AddResenas(ResenasModel model)
        {
            _context.resenas.Add(model);
            _context.SaveChanges();
            return model;
        }

        public bool UpdateResenas(ResenasModel model)
        {
            var entidad = _context.resenas.FirstOrDefault(r => r.id_resena == model.id_resena);
            if (entidad == null) return false;

            entidad.comentario = model.comentario;
            entidad.calificacion = model.calificacion;

            _context.SaveChanges();
            return true;
        }

        public bool DeleteResenas(int id)
        {
            var entidad = _context.resenas.FirstOrDefault(r => r.id_resena == id);
            if (entidad == null) return false;

            _context.resenas.Remove(entidad);
            _context.SaveChanges();
            return true;
        }

        // =====================================================
        // RESEÑAS PÚBLICAS CON JOIN COMPLETO
        // =====================================================

        public IEnumerable<object> GetResenasPublicas()
        {
            var data =
                from r in _context.resenas
                join usr in _context.Users
                    on r.cliente_id equals usr.Id
                join rev in _context.revision
                    on r.revision_id equals rev.id_revision
                join srvRev in _context.servicios_revision
                    on rev.id_revision equals srvRev.revision_id into srvJoin
                from srvRev in srvJoin.DefaultIfEmpty()
                join srv in _context.servicios
                    on srvRev.servicio_id equals srv.id_servicio into srv2Join
                from srv in srv2Join.DefaultIfEmpty()
                select new
                {
                    r.id_resena,
                    r.calificacion,
                    r.comentario,
                    usuario = usr.NombreCompleto ?? usr.UserName,
                    servicio = srv != null ? srv.nombre : "Servicio no registrado"
                };

            return data.ToList();
        }

        // =====================================================
        // VALIDACIONES
        // =====================================================

        public bool ValidarRevisionExiste(int revisionId)
        {
            return _context.revision.Any(r => r.id_revision == revisionId);
        }

        public bool EsRevisionDelCliente(int revisionId, int clienteId)
        {
            var revision = _context.revision
                .FirstOrDefault(r => r.id_revision == revisionId);

            if (revision == null) return false;

            var veh = _context.Vehiculos
                .FirstOrDefault(v => v.IdVehiculo == revision.vehiculo_id);

            // VALIDACIÓN REAL (según tu modelo Vehiculo)
            return veh != null && veh.ClienteId == clienteId;
        }

        public bool RevisionCompletada(int revisionId)
        {
            var revision = _context.revision.FirstOrDefault(r => r.id_revision == revisionId);
            if (revision == null) return false;

            // Ajustar si cambia, este ID corresponde a "Entregado"
            const int ESTADO_ENTREGADO = 7;

            return revision.id_estado == ESTADO_ENTREGADO;
        }

        public bool YaTieneResena(int revisionId)
        {
            return _context.resenas.Any(r => r.revision_id == revisionId);
        }

        // =====================================================
        // MÉTODO COMPLETO PARA AÑADIR RESEÑA CON VALIDACIONES
        // =====================================================

        public (bool ok, string? error, ResenasModel? nueva) AgregarConValidacion(ResenasModel model)
        {
            if (!ValidarRevisionExiste(model.revision_id))
                return (false, "La revisión no existe.", null);

            if (!EsRevisionDelCliente(model.revision_id, model.cliente_id))
                return (false, "No tiene permiso para reseñar esta revisión.", null);

            if (!RevisionCompletada(model.revision_id))
                return (false, "Solo puede reseñar revisiones completadas.", null);

            if (YaTieneResena(model.revision_id))
                return (false, "Esta revisión ya tiene una reseña.", null);

            _context.resenas.Add(model);
            _context.SaveChanges();

            return (true, null, model);
        }
    }
}
