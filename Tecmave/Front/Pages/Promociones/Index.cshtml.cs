using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tecmave.Front.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Front.Pages.Promociones
{
    [Authorize(Roles = "Admin,Colaborador")]
    public class PromocionesIndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PromocionesIndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<PromocionesModel> Promociones { get; set; } = new();

        private const string API_BASE = "https://tecmave-api.azurewebsites.net";
        private static string UrlPromociones => $"{API_BASE}/promociones"; // minúscula, consistente con [Route("promociones")]

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            Promociones = await client.GetFromJsonAsync<List<PromocionesModel>>(UrlPromociones)
                          ?? new List<PromocionesModel>();
        }

        public async Task<IActionResult> OnPostEnviarPromoAsync(int idPromocion)
        {
            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync($"{UrlPromociones}/enviar-promo/{idPromocion}", null);
            }
            catch
            {
                TempData["MensajeError"] = "No se pudo contactar el API para enviar la promoción.";
                return RedirectToPage();
            }

            // Leer mensaje siempre (ok o error)
            string mensaje = "Solicitud procesada.";
            try
            {
                var info = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
                if (info != null && info.TryGetValue("mensaje", out var msgObj) && msgObj != null)
                    mensaje = msgObj.ToString()!;
            }
            catch
            {
                // si no viene JSON, dejamos el mensaje default
            }

            if (!response.IsSuccessStatusCode)
            {
                TempData["MensajeError"] = string.IsNullOrWhiteSpace(mensaje)
                    ? "Ocurrió un error al enviar la promoción."
                    : mensaje;

                return RedirectToPage();
            }

            // Si el API dice “No se envió...”, NO lo mostramos como éxito
            var lower = (mensaje ?? "").ToLowerInvariant();
            var esNoEnvio =
                lower.Contains("no se envi") ||
                lower.Contains("no se envio") ||
                lower.Contains("notificados anteriormente");

            if (esNoEnvio)
            {
                TempData["MensajeError"] = mensaje; // lo tratamos como aviso/error para que no salga “check”
            }
            else
            {
                TempData["Mensaje"] = mensaje;
            }

            return RedirectToPage();
        }
    }
}
