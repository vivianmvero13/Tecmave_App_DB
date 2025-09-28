using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Models;

namespace Tecmave.Api.Data
{
    public class AppDbContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EstadosModel> Estados { get; set; }
        public DbSet<TipoServiciosModel> TipoServicios { get; set; }
        public DbSet<ModelosModel> Modelos { get; set; }
        public DbSet<MarcasModel> Marcas { get; set; }
        public DbSet<VehiculosModel> Vehiculos { get; set; }
        public DbSet<ServiciosModel> Servicios { get; set; }
        public DbSet<RevisionModel> Revisiones { get; set; }
        public DbSet<AgendamientoModel> Agendamientos { get; set; }
        public DbSet<FacturasModel> factura { get; set; }
        public DbSet<DetalleFacturaModel> detalle_factura { get; set; }
        public DbSet<ResenasModel> Resenas { get; set; }
        public DbSet<NotificacionesModel> Notificaciones { get; set; }
        public DbSet<PromocionesModel> Promociones { get; set; }
        public DbSet<ColaboradoresModel> Colaboradores { get; set; }
        public DbSet<ServiciosRevisionModel> servicios_revision { get; set; }




        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Usuario>().ToTable("aspnetusers");
            b.Entity<IdentityRole<int>>().ToTable("aspnetroles");
            b.Entity<IdentityUserRole<int>>().ToTable("aspnetuserroles");
            b.Entity<IdentityUserLogin<int>>().ToTable("aspnetuserlogins");
            b.Entity<IdentityUserToken<int>>().ToTable("aspnetusertokens");
            b.Entity<IdentityUserClaim<int>>().ToTable("aspnetuserclaims");
            b.Entity<IdentityRoleClaim<int>>().ToTable("aspnetroleclaims");

            b.Entity<EstadosModel>().ToTable("estados");
            b.Entity<TipoServiciosModel>().ToTable("tipo_servicios");
            b.Entity<ModelosModel>().ToTable("modelo");
            b.Entity<MarcasModel>().ToTable("marca");
            b.Entity<VehiculosModel>().ToTable("vehiculos");
            b.Entity<ServiciosModel>().ToTable("servicios");
            b.Entity<RevisionModel>().ToTable("revision");
            b.Entity<AgendamientoModel>().ToTable("agendamiento");
            b.Entity<FacturasModel>().ToTable("factura");
            b.Entity<FacturasModel>().ToTable("notificaciones");
            b.Entity<DetalleFacturaModel>().ToTable("DetalleFactura");
            b.Entity<PromocionesModel>().ToTable("promociones");

        }
    }
}
