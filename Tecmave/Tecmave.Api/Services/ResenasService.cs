using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class ResenasService
    {

        private readonly AppDbContext _context;

        public ResenasService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<ResenasModel> _canton = new List<ResenasModel>();
        private int _nextid_resena = 1;


        //funcion de obtener cantons
        public List<ResenasModel> GetResenasModel()
        {
            return _context.resenas.ToList();
        }


        public ResenasModel GetByid_resena(int id)
        {
            return _context.resenas.FirstOrDefault(p => p.id_resena == id);
        }

        public ResenasModel AddResenas(ResenasModel ResenasModel)
        {
            _context.resenas.Add(ResenasModel);
            _context.SaveChanges();
            return ResenasModel;
        }


        public bool UpdateResenas(ResenasModel ResenasModel)
        {
            var entidad = _context.resenas.FirstOrDefault(p => p.id_resena == ResenasModel.id_resena);

            if (entidad == null)
            {
                return false;
            }

            entidad.comentario = ResenasModel.comentario;


            _context.SaveChanges();

            return true;

        }


        public bool DeleteResenas(int id)
        {
            var entidad = _context.resenas.FirstOrDefault(p => p.id_resena == id);

            if (entidad == null)
            {
                return false;
            }

            _context.resenas.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
