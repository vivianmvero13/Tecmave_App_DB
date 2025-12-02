using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tecmave.Api.Models;

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

        // OJO: lista del modelo de API, NO de IndexModel
        public List<PromocionesModel> Promociones { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            Promociones = await client.GetFromJsonAsync<List<PromocionesModel>>(
                              "http://localhost:7096/Promociones")
                          ?? new List<PromocionesModel>();
        }

        public async Task<IActionResult> OnPostEnviarPromoAsync(int idPromocion)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync(
                $"http://localhost:7096/Promociones/enviar-promo/{idPromocion}",
                null);

            string mensaje = "Promoción enviada.";

            if (response.IsSuccessStatusCode)
            {
                var info = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (info != null && info.TryGetValue("mensaje", out var msg))
                {
                    mensaje = msg;
                }
            }
            else
            {
                mensaje = "Ocurrió un error al enviar la promoción.";
            }

            TempData["Mensaje"] = mensaje;
            return RedirectToPage();
        }
    }
}