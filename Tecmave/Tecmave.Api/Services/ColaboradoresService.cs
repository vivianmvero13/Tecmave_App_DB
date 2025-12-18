using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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

        //Aca necesitamos el modelo de datos para el almacenamiento temporal
        private readonly List<ColaboradoresModel> _canton = new List<ColaboradoresModel>();
        private int _nextid_colaborador = 1;


        //funcion de obtener cantons
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

            var usuario = new Usuario
            {
                Direccion = dto.Direccion,
                Cedula = dto.Cedula,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                UserName = dto.UserName,
                Email = dto.Email,
                Estado = 1,
                PhoneNumber = dto.Telefono

            };

            var resultado = await _userManager.CreateAsync(usuario, "123Abc!");
            if (!resultado.Succeeded)
            {
                throw new Exception("Error al crear el usuario: " +
                    string.Join(", ", resultado.Errors.Select(e => e.Description)));
            }

            
            if (!string.IsNullOrEmpty(dto.Rol))
            {
                await _userManager.AddToRoleAsync(usuario, dto.Rol);
            }

            
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
            var entidad = await _context.colaboradores
                .FirstOrDefaultAsync(c => c.id_colaborador == id);

            if (entidad == null)
                return false;

            // 1. Eliminar colaborador primero
            _context.colaboradores.Remove(entidad);
            await _context.SaveChangesAsync();

            // 2. Luego eliminar usuario Identity
            var usuario = await _userManager.FindByIdAsync(entidad.id_usuario.ToString());

            if (usuario != null)
            {
                var result = await _userManager.DeleteAsync(usuario);

                if (!result.Succeeded)
                    return false;

                _context.role_change_audit.Add(new RoleChangeAudit
                {
                    TargetUserId = usuario.Id,
                    TargetUserName = usuario.UserName,
                    PreviousRole = null,
                    NewRole = null,
                    ChangedByUserName = "Administrador",
                    ChangedAtUtc = DateTime.UtcNow,
                    Action = "Deleteuser"
                });

                await _context.SaveChangesAsync();
            }

            return true;
        }


    }
}
