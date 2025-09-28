using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Tecmave.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificacionesController : Controller
    {
        private readonly NotificacionesService _NotificacionesService;

        public NotificacionesController(NotificacionesService NotificacionesService)
        {
            _NotificacionesService = NotificacionesService;
        }

        //Apis GET, POST, PUT   y DELETE
        [HttpGet]
        public ActionResult<IEnumerable<NotificacionesModel>> GetNotificacionesModel()
        {
            return _NotificacionesService.GetNotificacionesModel();
        }

        [HttpGet("{id}")]
        public ActionResult<NotificacionesModel> GetById(int id)
        {
            return _NotificacionesService.GetByid_notificacion(id);
        }

        //Apis POST
        [HttpPost]
        public ActionResult<NotificacionesModel> AddNotificaciones(NotificacionesModel NotificacionesModel)
        {

            var newNotificacionesModel = _NotificacionesService.AddNotificaciones(NotificacionesModel);

            return
                CreatedAtAction(
                        nameof(GetNotificacionesModel), new
                        {
                            id = newNotificacionesModel.id_notificaciones,
                        },
                        newNotificacionesModel);

        }

        //APIS PUT
        [HttpPut]
        public IActionResult UpdateNotificaciones(NotificacionesModel NotificacionesModel)
        {

            if (!_NotificacionesService.UpdateNotificaciones(NotificacionesModel))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  notificacion no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

        //APIS DELETE
        [HttpDelete]
        public IActionResult DeleteNotificacionesModel(int id)
        {

            if (!_NotificacionesService.DeleteNotificaciones(id))
            {
                return NotFound(
                        new
                        {
                            elmsneaje = "La  notificacion no fue encontrado"
                        }
                    );
            }

            return NoContent();

        }

    }
}
