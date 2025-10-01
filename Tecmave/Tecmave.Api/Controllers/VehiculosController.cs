using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehiculosController : Controller
    {
        private readonly VehiculosService _VehiculosService;

        public VehiculosController(VehiculosService VehiculosService)
        {
            _VehiculosService = VehiculosService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<VehiculosModel>> GetVehiculosModel()
        {
            return _VehiculosService.GetVehiculosModel();
        }

        [HttpGet("{id}")]
        public ActionResult<VehiculosModel> GetById(int id)
        {
            return _VehiculosService.GetByid_vehiculo(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<VehiculosModel> AddVehiculos(VehiculosModel VehiculosModel)
        {

            var newVehiculosModel = _VehiculosService.AddVehiculos(VehiculosModel);

            return
                CreatedAtAction(
                        nameof(GetVehiculosModel), new
                        {
                            id = newVehiculosModel.id_vehiculo,
                        },
                        newVehiculosModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateVehiculos(VehiculosModel VehiculosModel)
        {

            if (!_VehiculosService.UpdateVehiculos(VehiculosModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El vehiculo no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteVehiculosModel(int id)
        {

            if (!_VehiculosService.DeleteVehiculos(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El vehiculo no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
