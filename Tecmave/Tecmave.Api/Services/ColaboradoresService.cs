using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Models.Dto;

namespace Tecmave.Api.Services
{
    public class ColaboradoresService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public ColaboradoresService(AppDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private bool ValidarCedula(string cedula)
        {
            return Regex.IsMatch(cedula, @"^\d{9}$");
        }

        private bool ValidarTelefono(string tel)
        {
            return Regex.IsMatch(tel, @"^\d{8}$");
        }

        private bool ValidarEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@]+@[^@]+\.[a-zA-Z]{2,}$");
        }

        public List<ColaboradoresModel> GetColaboradoresModel()
        {
            return _context.colaboradores.ToList();
        }

        public ColaboradoresModel GetByid_colaborador(int id)
        {
            return _context.colaboradores.FirstOrDefault(p => p.id_colaborador == id);
        }

        public async Task<ColaboradoresModel> AddColaboradoresAsync(Colaborador dto)
        {
            if (!ValidarCedula(dto.Cedula)) throw new Exception("Cédula inválida");
            if (!ValidarTelefono(dto.Telefono)) throw new Exception("Teléfono inválido");
            if (!ValidarEmail(dto.Email)) throw new Exception("Correo inválido");

            var usuario = new Usuario
            {
                Direccion = dto.Direccion,
                Cedula = dto.Cedula,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                UserName = dto.Email,
                Email = dto.Email,
                Estado = 1,
                PhoneNumber = dto.Telefono
            };

            var resultado = await _userManager.CreateAsync(usuario, "123Abc!");
            if (!resultado.Succeeded)
            {
                throw new Exception(string.Join(", ", resultado.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(usuario, "Colaborador");

            dto.Colaboradores.id_usuario = usuario.Id;

            _context.colaboradores.Add(dto.Colaboradores);
            await _context.SaveChangesAsync();

            return dto.Colaboradores;
        }


        public async Task<bool> UpdateColaboradorAsync(EditarColaboradorDto dto)
        {
            
            var colaborador = await _context.colaboradores
                .FirstOrDefaultAsync(c => c.id_colaborador == dto.IdColaborador);

            if (colaborador == null)
                return false;

            colaborador.puesto = dto.Puesto;
            colaborador.salario = dto.Salario;
            colaborador.fecha_contratacion = DateOnly.FromDateTime(dto.FechaContratacion);


            var usuario = await _userManager.FindByIdAsync(dto.IdUsuario.ToString());
            if (usuario == null)
                return false;

            usuario.Nombre = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Cedula = dto.Cedula;
           

            var userUpdate = await _userManager.UpdateAsync(usuario);
            if (!userUpdate.Succeeded)
                return false;

         

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteColaboradoresAsync(int id)
        {
            var entidad = await _context.colaboradores.FirstOrDefaultAsync(c => c.id_colaborador == id);
            if (entidad == null) return false;

            _context.colaboradores.Remove(entidad);
            await _context.SaveChangesAsync();

            var usuario = await _userManager.FindByIdAsync(entidad.id_usuario.ToString());
            if (usuario != null) await _userManager.DeleteAsync(usuario);

            return true;
        }
    }
}
