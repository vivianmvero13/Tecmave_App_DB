using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using ApiProm = Tecmave.Api.Models;


namespace Front.Pages.Promociones
{
    [Authorize]
    public class PromocionesPageModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PromocionesPageModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public List<ApiProm.PromocionesModel> Promociones { get; set; } = new();

        [BindProperty]
        public string EmailDestino { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            Promociones= await client.GetFromJsonAsync<List<ApiProm.PromocionesModel>>("http://localhost:7096/Promociones");

        }

        public async Task<IActionResult> OnPostEnviarAsync(int idUsuario)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(
                $"http://localhost:7096/Promociones/enviar/{idUsuario}?email={EmailDestino}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Las promociones fueron enviadas correctamente.";
            }

            else
            {
                TempData["Mensaje"] = "Se dio un error al enviar las promociones";
            }

            return RedirectToPage();
        }
    }
}
