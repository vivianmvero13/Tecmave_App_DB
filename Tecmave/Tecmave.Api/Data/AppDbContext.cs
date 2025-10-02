using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Models;

namespace Tecmave.Api.Data
{
    public class AppDbContext : IdentityDbContext<Usuario, AppRole, int,
        IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EstadosModel> estados { get; set; }
        public DbSet<TipoServiciosModel> tipo_servicios { get; set; }
        public DbSet<ModelosModel> modelo { get; set; }
        public DbSet<MarcasModel> marca { get; set; }
        public DbSet<VehiculosModel> vehiculos { get; set; }
        public DbSet<ServiciosModel> servicios { get; set; }
        public DbSet<RevisionModel> revision { get; set; }
        public DbSet<AgendamientoModel> agendamientos { get; set; }
        public DbSet<FacturasModel> factura { get; set; }
        public DbSet<DetalleFacturaModel> detalle_factura { get; set; }
        public DbSet<ResenasModel> resenas { get; set; }
        public DbSet<NotificacionesModel> notificaciones { get; set; }
        public DbSet<PromocionesModel> promociones { get; set; }
        public DbSet<ColaboradoresModel> colaboradores { get; set; }
        public DbSet<ServiciosRevisionModel> servicios_revision { get; set; }

        public DbSet<RoleChangeAudit> role_change_audit { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Usuario>().ToTable("aspnetusers");
            b.Entity<AppRole>().ToTable("aspnetroles");

            b.Entity<IdentityUserRole<int>>(e =>
            {
                e.ToTable("aspnetuserroles");
                e.HasKey(x => new { x.UserId, x.RoleId });

                e.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne<AppRole>()
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            b.Entity<IdentityUserLogin<int>>().ToTable("aspnetuserlogins");
            b.Entity<IdentityUserToken<int>>().ToTable("aspnetusertokens");
            b.Entity<IdentityUserClaim<int>>().ToTable("aspnetuserclaims");
            b.Entity<IdentityRoleClaim<int>>().ToTable("aspnetroleclaims");

            b.Entity<AppRole>(e =>
            {
                e.Property(r => r.Description).HasMaxLength(256);
                e.Property(r => r.IsActive).HasDefaultValue(true);
            });

            b.Entity<RoleChangeAudit>(e =>
            {
                e.ToTable("role_change_audit");
                e.HasKey(x => x.Id);

                e.Property(x => x.TargetUserId).IsRequired();
                e.Property(x => x.ChangedAtUtc).IsRequired();
                e.Property(x => x.Action).HasMaxLength(20).IsRequired();

                e.Property(x => x.TargetUserName).HasMaxLength(256);
                e.Property(x => x.PreviousRole).HasMaxLength(256);
                e.Property(x => x.NewRole).HasMaxLength(256);
                e.Property(x => x.ChangedByUserName).HasMaxLength(256);
                e.Property(x => x.Detail).HasMaxLength(1024);
                e.Property(x => x.SourceIp).HasMaxLength(64);

                e.HasIndex(x => x.TargetUserId);
                e.HasIndex(x => x.ChangedAtUtc);
                e.HasIndex(x => x.Action);
            });

            b.Entity<EstadosModel>().ToTable("estados").HasKey(x => x.id_estado);
            b.Entity<TipoServiciosModel>().ToTable("tipo_servicios").HasKey(x => x.id_tipo_servicio);
            b.Entity<ModelosModel>().ToTable("modelo").HasKey(x => x.id_modelo);
            b.Entity<MarcasModel>().ToTable("marca").HasKey(x => x.id_marca);
            b.Entity<VehiculosModel>().ToTable("vehiculos").HasKey(x => x.id_vehiculo);
            b.Entity<ServiciosModel>().ToTable("servicios").HasKey(x => x.id_servicio);
            b.Entity<RevisionModel>().ToTable("revision").HasKey(x => x.id_revision);
            b.Entity<AgendamientoModel>().ToTable("agendamiento").HasKey(x => x.id_agendamiento);
            b.Entity<FacturasModel>().ToTable("factura").HasKey(x => x.id_factura);
            b.Entity<DetalleFacturaModel>().ToTable("detalle_factura").HasKey(x => x.id_detalle);
            b.Entity<ResenasModel>().ToTable("resenas").HasKey(x => x.id_resena);
            b.Entity<NotificacionesModel>().ToTable("notificaciones").HasKey(x => x.id_notificaciones);
            b.Entity<PromocionesModel>().ToTable("promociones").HasKey(x => x.id_promocion);
            b.Entity<ColaboradoresModel>().ToTable("colaboradores").HasKey(x => x.id_colaborador);
            b.Entity<ServiciosRevisionModel>().ToTable("servicios_revision").HasKey(x => x.id_servicio_revision);
        }
    }
}
