using System.Linq;
using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class FacturasService
    {
        private readonly AppDbContext _context;

        public FacturasService(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<FacturasModel> GetAll()
        {
            return _context.factura.AsQueryable();
        }

        public FacturasModel? GetById(int id)
        {
            return _context.factura.FirstOrDefault(f => f.id_factura == id);
        }

        public bool UpdateEstado(int id, string nuevoEstado)
        {
            var f = _context.factura.FirstOrDefault(x => x.id_factura == id);
            if (f == null) return false;
            f.estado_pago = nuevoEstado;
            _context.SaveChanges();
            return true;
        }

        public FacturasModel Add(FacturasModel model)
        {
            if (model.fecha_emision == null) model.fecha_emision = System.DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(model.estado_pago)) model.estado_pago = "Pendiente";
            _context.factura.Add(model);
            _context.SaveChanges();
            return model;
        }

        public FacturasModel? GenerateFromService(int clienteId, int servicioId, string? metodoPago = null)
        {
            var servicio = _context.servicios.FirstOrDefault(s => s.id_servicio == servicioId);
            if (servicio == null) return null;
            var factura = new FacturasModel
            {
                cliente_id = clienteId,
                fecha_emision = System.DateTime.UtcNow,
                total = servicio.precio,
                metodo_pago = string.IsNullOrWhiteSpace(metodoPago) ? "Pendiente" : metodoPago,
                estado_pago = "Pendiente"
            };
            _context.factura.Add(factura);
            _context.SaveChanges();
            return factura;
        }
    }
}
