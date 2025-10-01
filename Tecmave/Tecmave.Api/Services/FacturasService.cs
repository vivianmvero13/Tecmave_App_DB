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

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<FacturasModel> _canton = new List<FacturasModel>();
        private int _nextid_factura = 1;


        //funcion de obtener cantons
        public List<FacturasModel> GetFacturasModel()
        {
            return _context.factura.ToList();
        }


        public FacturasModel GetByid_factura(int id)
        {
            return _context.factura.FirstOrDefault(p => p.id_factura == id);
        }

        public FacturasModel AddFacturas(FacturasModel FacturasModel)
        {
            _context.factura.Add(FacturasModel);
            _context.SaveChanges();
            return FacturasModel;
        }


        public bool UpdateFacturas(FacturasModel FacturasModel)
        {
            var entidad = _context.factura.FirstOrDefault(p => p.id_factura == FacturasModel.id_factura);

            if (entidad == null)
            {
                return false;
            }

            entidad.total = FacturasModel.id_factura;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteFacturas(int id)
        {
            var entidad = _context.factura.FirstOrDefault(p => p.id_factura == id);

            if (entidad == null)
            {
                return false;
            }

            _context.factura.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
