using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class MarcasService
    {

        private readonly AppDbContext _context;

        public MarcasService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<MarcasModel> _canton = new List<MarcasModel>();
        private int _nextid_marca = 1;


        //funcion de obtener cantons
        public List<MarcasModel> GetMarcasModel()
        {
            return _context.Marcas.ToList();
        }


        public MarcasModel GetByid_marca(int id)
        {
            return _context.Marcas.FirstOrDefault(p => p.id_marca == id);
        }

        public MarcasModel AddMarcas(MarcasModel MarcasModel)
        {
            _context.Marcas.Add(MarcasModel);
            _context.SaveChanges();
            return MarcasModel;
        }


        public bool UpdateMarcas(MarcasModel MarcasModel)
        {
            var entidad = _context.Marcas.FirstOrDefault(p => p.id_marca == MarcasModel.id_marca);

            if (entidad == null)
            {
                return false;
            }

            entidad.nombre = MarcasModel.nombre;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteMarcas(int id)
        {
            var entidad = _context.Marcas.FirstOrDefault(p => p.id_marca == id);

            if (entidad == null)
            {
                return false;
            }

            _context.Marcas.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
