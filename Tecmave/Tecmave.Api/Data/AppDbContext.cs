using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Models;

namespace Tecmave.Api.Data
{
    //creamos el contexto de la conexion de la db
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<AgendamientoModel> Agendamiento { get; set; }
        public DbSet<TipoServiciosModel> tipo_servicios { get; set; }
        public DbSet<ServiciosModel> servicios { get; set; }
        public DbSet<RevisionModel> revision { get; set; }


    }

}
