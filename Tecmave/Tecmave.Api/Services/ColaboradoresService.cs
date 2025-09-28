using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class ColaboradoresService
    {

        private readonly AppDbContext _context;

        public ColaboradoresService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<ColaboradoresModel> _canton = new List<ColaboradoresModel>();
        private int _nextid_colaborador = 1;


        //funcion de obtener cantons
        public List<ColaboradoresModel> GetColaboradoresModel()
        {
            return _context.Colaboradores.ToList();
        }


        public ColaboradoresModel GetByid_colaborador(int id)
        {
            return _context.Colaboradores.FirstOrDefault(p => p.id_colaborador == id);
        }

        public ColaboradoresModel AddColaboradores(ColaboradoresModel ColaboradoresModel)
        {
            _context.Colaboradores.Add(ColaboradoresModel);
            _context.SaveChanges();
            return ColaboradoresModel;
        }


        public bool UpdateColaboradores(ColaboradoresModel ColaboradoresModel)
        {
            var entidad = _context.Colaboradores.FirstOrDefault(p => p.id_colaborador == ColaboradoresModel.id_colaborador);

            if (entidad == null)
            {
                return false;
            }

            entidad.id_usuario = ColaboradoresModel.id_usuario;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteColaboradores(int id)
        {
            var entidad = _context.Colaboradores.FirstOrDefault(p => p.id_colaborador == id);

            if (entidad == null)
            {
                return false;
            }

            _context.Colaboradores.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
