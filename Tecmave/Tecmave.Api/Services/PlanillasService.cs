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

            entidad.horas_trabajadas = PlanillasModel.horas_trabajadas;
            entidad.valor_hora = PlanillasModel.valor_hora;
            entidad.total_salario = PlanillasModel.total_salario;
            entidad.neto_pagar = PlanillasModel.neto_pagar;
            entidad.estado = PlanillasModel.estado;
            entidad.observaciones = PlanillasModel.observaciones;


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

        public PlanillasModel RegistrarHoras(int id, decimal horas_extra)
        {
            var planilla = _context.planillas.FirstOrDefault(p => p.id == id);

            if (planilla == null)
            {
                return null;
            }

            planilla.horas_trabajadas += horas_extra;
            planilla.total_salario = planilla.horas_trabajadas * planilla.valor_hora;
            
            _context.SaveChanges();
            return planilla;
        }

        public PlanillasModel AjustarPago(int id, decimal neto_pagar)
        {
            var planilla = _context.planillas.FirstOrDefault(p => p.id == id);
            if (planilla == null)
            {
                return null;
            }

            planilla.neto_pagar = neto_pagar;
            _context.SaveChanges();
            return planilla;
        }

        public bool AprobarPlanilla(int id)
        {
            var planilla = _context.planillas.FirstOrDefault(p => p.id == id);
            if (planilla == null)
            {
                return false;
            }

            planilla.estado = "Aprobada";
            _context.SaveChanges();
            return true;
        }

    }
}
