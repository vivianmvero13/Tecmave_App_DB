using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : Controller
    {
        private readonly RolesService _RolesService;

        public RolesController(RolesService RolesService)
        {
            _RolesService = RolesService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<RolesModel>> GetRolesModel()
        {
            return _RolesService.GetRolesModel();
        }

        [HttpGet("{id}")]
        public ActionResult<RolesModel> GetById(int id)
        {
            return _RolesService.GetById(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<RolesModel> AddRoles(RolesModel RolesModel)
        {

            var newRolesModel = _RolesService.AddRoles(RolesModel);

            return
                CreatedAtAction(
                        nameof(GetRolesModel), new
                        {
                            id = newRolesModel.Id,
                        },
                        newRolesModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateRoles(RolesModel RolesModel)
        {

            if (!_RolesService.UpdateRoles(RolesModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de rol no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteRolesModel(int id)
        {

            if (!_RolesService.DeleteRoles(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "El  de rol no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
