using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class UsuarioService
    {

        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<UsuariosModel> _usuario = new List<UsuariosModel>();
        private int _nextId = 1;


        //funcion de obtener usuarios
        public List<UsuariosModel> GetUsuariosModel()
        { 
                return _context.aspnetusers.ToList(); 
        }


        public UsuariosModel GetById(int id) {
            return _context.aspnetusers.FirstOrDefault(p=> p.Id== id);
        }

        public UsuariosModel AddUsuario(UsuariosModel UsuariosModel)
        {
            _context.aspnetusers.Add(UsuariosModel);
            _context.SaveChanges();
            return UsuariosModel;
        }


        public bool UpdateUsuario(UsuariosModel UsuariosModel)
        {
            var entidad =  _context.aspnetusers.FirstOrDefault(p => p.Id== UsuariosModel.Id);

            if (entidad == null) {
                return false;
            }

            _context.SaveChanges();

            return true;

        }


        public bool DeleteUsuario(int id)
        {
            var entidad = _context.aspnetusers.FirstOrDefault(p => p.Id== id);

            if (entidad == null)
            {
                return false;
            }

            _context.aspnetusers.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
