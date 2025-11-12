using Microsoft.AspNetCore.Mvc;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DetalleFacturaController : Controller
    {
        private readonly DetalleFacturaService _DetalleFacturaService;

        public DetalleFacturaController(DetalleFacturaService DetalleFacturaService)
        {
            _DetalleFacturaService = DetalleFacturaService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<DetalleFacturaModel>> GetDetalleFacturaModel()
        {
            return _DetalleFacturaService.GetDetalleFacturaModel();
        }

        [HttpGet("{id}")]
        public ActionResult<DetalleFacturaModel> GetById(int id)
        {
            return _DetalleFacturaService.GetByid_detalle(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<DetalleFacturaModel> AddDetalleFactura(DetalleFacturaModel DetalleFacturaModel)
        {

            var newDetalleFacturaModel = _DetalleFacturaService.AddDetalleFactura(DetalleFacturaModel);

            return
                CreatedAtAction(
                        nameof(GetDetalleFacturaModel), new
                        {
                            id = newDetalleFacturaModel.id_detalle,
                        },
                        newDetalleFacturaModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateDetalleFactura(DetalleFacturaModel DetalleFacturaModel)
        {

            if (!_DetalleFacturaService.UpdateDetalleFactura(DetalleFacturaModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  detalle no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteDetalleFacturaModel(int id)
        {

            if (!_DetalleFacturaService.DeleteDetalleFactura(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  detalle no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
