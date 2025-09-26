using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class RolesService
    {

        private readonly AppDbContext _context;

        public RolesService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<RolesModel> _canton = new List<RolesModel>();
        private int _nextId = 1;


        //funcion de obtener cantons
        public List<RolesModel> GetRolesModel()
        { 
                return _context.aspnetroles.ToList(); 
        }


        public RolesModel GetById(int id) {
            return _context.aspnetroles.FirstOrDefault(p=> p.Id == id);
        }

        public RolesModel AddRoles(RolesModel RolesModel)
        {
            _context.aspnetroles.Add(RolesModel);
            _context.SaveChanges();
            return RolesModel;
        }


        public bool UpdateRoles(RolesModel RolesModel)
        {
            var entidad =  _context.aspnetroles.FirstOrDefault(p => p.Id == RolesModel.Id);

            if (entidad == null) {
                return false;
            }

            entidad.Name = RolesModel.Name;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteRoles(int id)
        {
            var entidad = _context.aspnetroles.FirstOrDefault(p => p.Id == id);

            if (entidad == null)
            {
                return false;
            }

            _context.aspnetroles.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
