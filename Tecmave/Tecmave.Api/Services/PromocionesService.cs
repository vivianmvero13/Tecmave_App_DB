using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Services
{
    public class PromocionesService
    {

        private readonly AppDbContext _context;

        public PromocionesService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<PromocionesModel> _canton = new List<PromocionesModel>();
        private int _nextid_promocion = 1;


        //funcion de obtener cantons
        public List<PromocionesModel> GetPromocionesModel()
        {
            return _context.promociones.ToList();
        }


        public PromocionesModel GetByid_promocion(int id)
        {
            return _context.promociones.FirstOrDefault(p => p.id_promocion == id);
        }

        public PromocionesModel AddPromociones(PromocionesModel PromocionesModel)
        {
            _context.promociones.Add(PromocionesModel);
            _context.SaveChanges();
            return PromocionesModel;
        }


        public bool UpdatePromociones(PromocionesModel PromocionesModel)
        {
            var entidad = _context.promociones.FirstOrDefault(p => p.id_promocion == PromocionesModel.id_promocion);

            if (entidad == null)
            {
                return false;
            }

            entidad.titulo = PromocionesModel.titulo;


            _context.SaveChanges();

            return true;

        }


        public bool DeletePromociones(int id)
        {
            var entidad = _context.promociones.FirstOrDefault(p => p.id_promocion == id);

            if (entidad == null)
            {
                return false;
            }

            _context.promociones.Remove(entidad);
            _context.SaveChanges();
            return true;

        }


    }
}
