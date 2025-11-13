using Tecmave.Api.Data;
using Tecmave.Api.Models;

namespace Tecmave.Api.Services
{
    public class PlanillasService
    {
        private readonly AppDbContext _context;

        public PlanillasService(AppDbContext context)
        {
            _context = context;
        }

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<PlanillasModel> _canton = new List<PlanillasModel>();
        private int _nextid_colaborador = 1;


        //funcion de obtener cantons
        public List<PlanillasModel> GetPlanillasModel()
        {
            return _context.planillas.ToList();
        }


        public PlanillasModel GetByid_colaborador(int id)
        {
            return _context.planillas.FirstOrDefault(p => p.id == id);
        }

        public PlanillasModel AddPlanillas(PlanillasModel PlanillasModel)
        {
            _context.planillas.Add(PlanillasModel);
            _context.SaveChanges();
            return PlanillasModel;
        }


        public bool UpdatePlanillas(PlanillasModel PlanillasModel)
        {
            var entidad = _context.planillas.FirstOrDefault(p => p.id == PlanillasModel.id);

            if (entidad == null)
            {
                return false;
            }

            entidad.id = PlanillasModel.id;


            _context.SaveChanges();

            return true;

        }


        public bool DeletePlanillas(int id)
        {
            var entidad = _context.planillas.FirstOrDefault(p => p.id == id);

            if (entidad == null)
            {
                return false;
            }

            _context.planillas.Remove(entidad);
            _context.SaveChanges();
            return true;

        }

    }
}
