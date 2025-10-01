using System.Net.Http.Json;
using Tecmave.Mvc.Models;

namespace Tecmave.Mvc.Services
{
    public class AdminApiClient
    {
        private readonly HttpClient _http;

        public AdminApiClient(HttpClient http)
        {
            _http = http;
        }

        // Obtener lista de usuarios
        public async Task<List<UsuarioDto>> GetUsersAsync()
        {
            return await _http.GetFromJsonAsync<List<UsuarioDto>>("/api/usuarios") ?? new();
        }

        // Obtener un usuario por ID
        public async Task<UsuarioDto?> GetUserAsync(int id)
        {
            return await _http.GetFromJsonAsync<UsuarioDto>($"/api/usuarios/{id}");
        }

        // Obtener roles de un usuario
        public async Task<List<string>> GetUserRolesAsync(int id)
        {
            return await _http.GetFromJsonAsync<List<string>>($"/api/usuarios/{id}/roles") ?? new();
        }

        // Asignar rol
        public async Task<bool> AddRoleAsync(int userId, string role)
        {
            var res = await _http.PostAsync($"/api/usuarios/{userId}/roles/add?role={Uri.EscapeDataString(role)}", null);
            return res.IsSuccessStatusCode;
        }

        // Quitar rol
        public async Task<bool> RemoveRoleAsync(int userId, string role)
        {
            var res = await _http.PostAsync($"/api/usuarios/{userId}/roles/remove?role={Uri.EscapeDataString(role)}", null);
            return res.IsSuccessStatusCode;
        }
    }
}
