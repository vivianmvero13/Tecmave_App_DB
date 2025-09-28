using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class ModelosService
    {

        private readonly AppDbContext _context;

        public ModelosService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<ModelosModel> _canton = new List<ModelosModel>();
        private int _nextid_modelo = 1;


        //funcion de obtener cantons
        public List<ModelosModel> GetModelosModel()
        { 
                return _context.Modelos.ToList(); 
        }


        public ModelosModel GetByid_modelo(int id) {
            return _context.Modelos.FirstOrDefault(p=> p.id_modelo == id);
        }

        public ModelosModel AddModelos(ModelosModel ModelosModel)
        {
            _context.Modelos.Add(ModelosModel);
            _context.SaveChanges();
            return ModelosModel;
        }


        public bool UpdateModelos(ModelosModel ModelosModel)
        {
            var entidad =  _context.Modelos.FirstOrDefault(p => p.id_modelo == ModelosModel.id_modelo);

            if (entidad == null) {
                return false;
            }

            entidad.nombre = ModelosModel.nombre;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteModelos(int id)
        {
            var entidad = _context.Modelos.FirstOrDefault(p => p.id_modelo == id);

            if (entidad == null)
            {
                return false;
            }

            _context.Modelos.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
