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

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            Promociones = await client.GetFromJsonAsync<List<PromocionesModel>>(
                              "https://tecmave-api.azurewebsites.net/promociones")
                          ?? new List<PromocionesModel>();
        }

        public async Task<IActionResult> OnPostEnviarPromoAsync(int idPromocion)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync(
                $"https://tecmave-api.azurewebsites.net/promociones/enviar-promo/{idPromocion}",
                null
            );

            string mensaje = "Solicitud procesada.";

            if (response.Content.Headers.ContentType?.MediaType?.ToLower().Contains("application/json") == true)
            {
                var info = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

                if (info != null && info.TryGetValue("mensaje", out var msg) && !string.IsNullOrWhiteSpace(msg))
                {
                    mensaje = msg;
                }
            }
            else
            {
                mensaje = await response.Content.ReadAsStringAsync();
            }

            return new JsonResult(new { mensaje, ok = response.IsSuccessStatusCode });
        }

    }
}
