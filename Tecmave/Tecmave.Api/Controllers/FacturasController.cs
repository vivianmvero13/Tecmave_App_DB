using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturasController : Controller
    {
        private readonly FacturasService _FacturasService;

        public FacturasController(FacturasService FacturasService)
        {
            _FacturasService = FacturasService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<FacturasModel>> GetFacturasModel()
        {
            return _FacturasService.GetFacturasModel();
        }

        [HttpGet("{id}")]
        public ActionResult<FacturasModel> GetById(int id)
        {
            return _FacturasService.GetByid_factura(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<FacturasModel> AddFacturas(FacturasModel FacturasModel)
        {

            var newFacturasModel = _FacturasService.AddFacturas(FacturasModel);

            return
                CreatedAtAction(
                        nameof(GetFacturasModel), new
                        {
                            id = newFacturasModel.id_factura,
                        },
                        newFacturasModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateFacturas(FacturasModel FacturasModel)
        {

            if (!_FacturasService.UpdateFacturas(FacturasModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  factura no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteFacturasModel(int id)
        {

            if (!_FacturasService.DeleteFacturas(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  factura no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
