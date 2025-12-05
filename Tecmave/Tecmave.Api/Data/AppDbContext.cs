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
        public DbSet<MarcasModel> marca { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<ServiciosModel> servicios { get; set; }
        public DbSet<RevisionModel> revision { get; set; }
        public DbSet<AgendamientoModel> agendamientos { get; set; }
        public DbSet<FacturasModel> factura { get; set; }
        public DbSet<DetalleFacturaModel> detalle_factura { get; set; }
        public DbSet<ResenasModel> resenas { get; set; }
        public DbSet<NotificacionesModel> notificaciones { get; set; }
        public DbSet<ColaboradoresModel> colaboradores { get; set; }
        public DbSet<ServiciosRevisionModel> servicios_revision { get; set; }
        public DbSet<RevisionPertenenciasModel> revision_pertenencias { get; set; }
        public DbSet<RevisionTrabajosModel> revision_trabajos { get; set; }

        public DbSet<RoleChangeAudit> role_change_audit { get; set; }
        public DbSet<PromocionesModel> promociones { get; set; }

        public DbSet<PromocionEnvio> promocion_envios { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Recordatorio> recordatorios { get; set; }
        public DbSet<MantenimientoModel> Mantenimientos { get; set; }
        public DbSet<PlanillasModel> planillas { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // ---------------- IDENTITY ----------------
            b.Entity<Usuario>().ToTable("aspnetusers");
            b.Entity<AppRole>().ToTable("aspnetroles");

            b.Entity<IdentityUserRole<int>>(e =>
            {
                e.ToTable("aspnetuserroles");
                e.HasKey(x => new { x.UserId, x.RoleId });

                e.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne<AppRole>()
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
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

            b.Entity<Usuario>(e =>
            {
                e.Property(u => u.Nombre).HasMaxLength(50);
                e.Property(u => u.Apellido).HasMaxLength(50);
            });

            // ---------------- AUDITORÍA ROLES ----------------
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

            // ---------------- TABLAS DE NEGOCIO (básico) ----------------
            b.Entity<EstadosModel>().ToTable("estados").HasKey(x => x.id_estado);
            b.Entity<TipoServiciosModel>().ToTable("tipo_servicios").HasKey(x => x.id_tipo_servicio);
            b.Entity<MarcasModel>().ToTable("marca").HasKey(x => x.id_marca);
            b.Entity<Vehiculo>().ToTable("vehiculos").HasKey(x => x.IdVehiculo);
            b.Entity<ServiciosModel>().ToTable("servicios").HasKey(x => x.id_servicio);
            b.Entity<RevisionModel>().ToTable("revision").HasKey(x => x.id_revision);
            b.Entity<FacturasModel>().ToTable("factura").HasKey(x => x.id_factura);
            b.Entity<DetalleFacturaModel>().ToTable("detalle_factura").HasKey(x => x.id_detalle);
            b.Entity<ResenasModel>().ToTable("resenas").HasKey(x => x.id_resena);
            b.Entity<NotificacionesModel>().ToTable("notificaciones").HasKey(x => x.id_notificaciones);
            b.Entity<PromocionesModel>().ToTable("promociones").HasKey(x => x.id_promocion);
            b.Entity<ColaboradoresModel>().ToTable("colaboradores").HasKey(x => x.id_colaborador);
            b.Entity<ServiciosRevisionModel>().ToTable("servicios_revision").HasKey(x => x.id_servicio_revision);
            b.Entity<PromocionEnvio>().ToTable("promocion_envios").HasKey(x => x.IdEnvio);
            b.Entity<Recordatorio>().ToTable("recordatorios").HasKey(x => x.Id);
            b.Entity<PlanillasModel>().ToTable("planillas").HasKey(x => x.id);
            b.Entity<RevisionPertenenciasModel>().ToTable("revision_pertenencias").HasKey(x => x.id_pertenencia);
            b.Entity<RevisionTrabajosModel>().ToTable("revision_trabajos").HasKey(x => x.id_trabajo);

            // ---------------- MANTENIMIENTOS ----------------
            b.Entity<MantenimientoModel>(e =>
            {
                e.ToTable("Mantenimientos");

                e.HasKey(x => x.IdMantenimiento);

                e.Property(x => x.IdMantenimiento).HasColumnName("IdMantenimiento");
                e.Property(x => x.IdVehiculo).HasColumnName("IdVehiculo");
                e.Property(x => x.FechaMantenimiento).HasColumnName("FechaMantenimiento");
                e.Property(x => x.ProximoMantenimiento).HasColumnName("ProximoMantenimiento");
                e.Property(x => x.RecordatorioEnviado).HasColumnName("RecordatorioEnviado");
                e.Property(x => x.IdEstado).HasColumnName("IdEstado");

                e.HasOne(x => x.Vehiculo)
                 .WithMany()
                 .HasForeignKey(x => x.IdVehiculo)
                 .HasConstraintName("FK_Mantenimientos_vehiculos_IdVehiculo");


                e.HasOne(x => x.Estado)
                 .WithMany()
                 .HasForeignKey(x => x.IdEstado)
                 .HasConstraintName("FK_Mantenimientos_Estado");

            });

            // ---------------- AGENDAMIENTO ----------------
            b.Entity<AgendamientoModel>(e =>
            {
                e.ToTable("agendamiento");
                e.HasKey(x => x.id_agendamiento);

                e.Property(x => x.id_agendamiento).HasColumnName("id_agendamiento");
                e.Property(x => x.cliente_id).HasColumnName("cliente_id");
                e.Property(x => x.vehiculo_id).HasColumnName("vehiculo_id");
                e.Property(x => x.id_estado).HasColumnName("id_estado");
                e.Property(x => x.fecha_agregada).HasColumnName("fecha_agregada");
                e.Property(x => x.fecha_estimada).HasColumnName("fecha_estimada");
                e.Property(x => x.hora_llegada).HasColumnName("hora_llegada");

                // FK Agendamiento
                e.HasOne<Vehiculo>()
     .WithMany()
     .HasForeignKey(x => x.vehiculo_id)
     .HasConstraintName("FK_Agendamiento_Vehiculo")
     .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}