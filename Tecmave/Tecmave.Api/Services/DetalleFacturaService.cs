using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class DetalleFacturaService
    {

        private readonly AppDbContext _context;

        public DetalleFacturaService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<DetalleFacturaModel> _canton = new List<DetalleFacturaModel>();
        private int _nextid_detalle = 1;


        //funcion de obtener cantons
        public List<DetalleFacturaModel> GetDetalleFacturaModel()
        {
            return _context.detalle_factura.ToList();
        }


        public DetalleFacturaModel GetByid_detalle(int id)
        {
            return _context.detalle_factura.FirstOrDefault(p => p.id_detalle == id);
        }

        public DetalleFacturaModel AddDetalleFactura(DetalleFacturaModel DetalleFacturaModel)
        {
            _context.detalle_factura.Add(DetalleFacturaModel);
            _context.SaveChanges();
            return DetalleFacturaModel;
        }


        public bool UpdateDetalleFactura(DetalleFacturaModel DetalleFacturaModel)
        {
            var entidad = _context.detalle_factura.FirstOrDefault(p => p.id_detalle == DetalleFacturaModel.id_detalle);

            if (entidad == null)
            {
                return false;
            }

            entidad.descripcion = DetalleFacturaModel.descripcion;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteDetalleFactura(int id)
        {
            var entidad = _context.detalle_factura.FirstOrDefault(p => p.id_detalle == id);

            if (entidad == null)
            {
                return false;
            }

            _context.detalle_factura.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
