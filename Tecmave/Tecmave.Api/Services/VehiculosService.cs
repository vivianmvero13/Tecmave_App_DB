using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class VehiculosService
    {

        private readonly AppDbContext _context;

        public VehiculosService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<VehiculosModel> _canton = new List<VehiculosModel>();
        private int _nextid_vehiculo = 1;


        //funcion de obtener cantons
        public List<VehiculosModel> GetVehiculosModel()
        {
            return _context.vehiculos.ToList();
        }


        public VehiculosModel GetByid_vehiculo(int id)
        {
            return _context.vehiculos.FirstOrDefault(p => p.id_vehiculo == id);
        }

        public VehiculosModel AddVehiculos(VehiculosModel VehiculosModel)
        {
            _context.vehiculos.Add(VehiculosModel);
            _context.SaveChanges();
            return VehiculosModel;
        }


        public bool UpdateVehiculos(VehiculosModel VehiculosModel)
        {
            var entidad = _context.vehiculos.FirstOrDefault(p => p.id_vehiculo == VehiculosModel.id_vehiculo);

            if (entidad == null)
            {
                return false;
            }

            entidad.placa = VehiculosModel.placa;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteVehiculos(int id)
        {
            var entidad = _context.vehiculos.FirstOrDefault(p => p.id_vehiculo == id);

            if (entidad == null)
            {
                return false;
            }

            _context.vehiculos.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
