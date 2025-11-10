using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Models;

namespace Tecmave.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets (dominio)
        public DbSet<EstadosModel> estados { get; set; } = default!;
        public DbSet<TipoServiciosModel> tipo_servicios { get; set; } = default!;
        public DbSet<MarcasModel> marca { get; set; } = default!;
        public DbSet<Vehiculo> Vehiculos { get; set; } = default!;
        public DbSet<ServiciosModel> servicios { get; set; } = default!;
        public DbSet<RevisionModel> revision { get; set; } = default!;
        public DbSet<AgendamientoModel> agendamientos { get; set; } = default!;
        public DbSet<FacturasModel> factura { get; set; } = default!;
        public DbSet<DetalleFacturaModel> detalle_factura { get; set; } = default!;
        public DbSet<ResenasModel> resenas { get; set; } = default!;
        public DbSet<NotificacionesModel> notificaciones { get; set; } = default!;
        public DbSet<PromocionesModel> promociones { get; set; } = default!;
        public DbSet<ColaboradoresModel> colaboradores { get; set; } = default!;
        public DbSet<ServiciosRevisionModel> servicios_revision { get; set; } = default!;
        public DbSet<RoleChangeAudit> role_change_audit { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // =============================
            // Auditoría (tabla propia)
            // =============================
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

            // =============================
            // Tablas de dominio
            // =============================
            b.Entity<EstadosModel>().ToTable("estados").HasKey(x => x.id_estado);
            b.Entity<EstadosModel>(e =>
            {
                e.Property(x => x.nombre).HasMaxLength(255).IsRequired();
            });

            b.Entity<TipoServiciosModel>().ToTable("tipo_servicios").HasKey(x => x.id_tipo_servicio);
            b.Entity<TipoServiciosModel>(e =>
            {
                e.Property(x => x.nombre).HasMaxLength(100).IsRequired();
                e.Property(x => x.descripcion).HasMaxLength(100).IsRequired();
            });

            b.Entity<MarcasModel>().ToTable("marca").HasKey(x => x.id_marca);
            b.Entity<MarcasModel>(e =>
            {
                e.Property(x => x.nombre).HasMaxLength(255).IsRequired();
            });

            b.Entity<ServiciosModel>().ToTable("servicios").HasKey(x => x.id_servicio);
            b.Entity<ServiciosModel>(e =>
            {
                e.Property(x => x.nombre).HasMaxLength(150).IsRequired();
                e.Property(x => x.descripcion).HasMaxLength(150).IsRequired();
                e.Property(x => x.tipo).HasMaxLength(150).IsRequired();
                e.Property(x => x.precio).HasPrecision(10, 2);

                e.Property(x => x.tipo_servicio_id).HasColumnName("tipo_servicio_id");
                e.HasOne<TipoServiciosModel>()
                    .WithMany()
                    .HasForeignKey(x => x.tipo_servicio_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<Vehiculo>().ToTable("vehiculos").HasKey(x => x.IdVehiculo);
            b.Entity<Vehiculo>(e =>
            {
                e.ToTable("vehiculos");
                e.HasKey(x => x.IdVehiculo);

                // columnas
                e.Property(x => x.Anno).HasColumnName("anno");
                e.Property(x => x.Placa).HasColumnName("placa").HasMaxLength(255).IsRequired();
                e.Property(x => x.Modelo).HasColumnName("modelo").HasMaxLength(255);

                e.Property(x => x.IdMarca).HasColumnName("id_marca");
                e.Property(x => x.ClienteId).HasColumnName("cliente_id"); // FK INT a aspnetusers.Id (sin navegación)

                // relación a marca
                e.HasOne<MarcasModel>()
                    .WithMany()
                    .HasForeignKey(x => x.IdMarca)
                    .OnDelete(DeleteBehavior.Restrict);

                // IMPORTANTE: no modelamos navegación a Usuario para no tocar aspnetusers
            });

            b.Entity<FacturasModel>().ToTable("factura").HasKey(x => x.id_factura);
            b.Entity<FacturasModel>(e =>
            {
                e.Property(x => x.total).HasPrecision(10, 2);
                e.Property(x => x.metodo_pago).HasMaxLength(100);

                e.Property(x => x.cliente_id).HasColumnName("cliente_id"); // FK INT a aspnetusers.Id
                // Sin navegación a Usuario
            });

            b.Entity<DetalleFacturaModel>().ToTable("detalle_factura").HasKey(x => x.id_detalle);
            b.Entity<DetalleFacturaModel>(e =>
            {
                e.Property(x => x.costo).HasPrecision(10, 2);
                e.Property(x => x.subtotal).HasPrecision(10, 2);

                e.Property(x => x.factura_id).HasColumnName("factura_id");
                e.Property(x => x.servicio_id).HasColumnName("servicio_id");

                e.HasOne<FacturasModel>()
                    .WithMany()
                    .HasForeignKey(x => x.factura_id)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne<ServiciosModel>()
                    .WithMany()
                    .HasForeignKey(x => x.servicio_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<ResenasModel>().ToTable("resenas").HasKey(x => x.id_resena);
            b.Entity<ResenasModel>(e =>
            {
                e.Property(x => x.comentario).IsRequired();

                e.Property(x => x.cliente_id).HasColumnName("cliente_id"); // FK INT a aspnetusers.Id
                e.Property(x => x.servicio_id).HasColumnName("servicio_id");
                e.Property(x => x.fecha)
                 .HasColumnType("datetime(6)")
                 .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                // Sin navegación a Usuario
                e.HasOne<ServiciosModel>()
                    .WithMany()
                    .HasForeignKey(x => x.servicio_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<NotificacionesModel>().ToTable("notificaciones").HasKey(x => x.id_notificaciones);
            b.Entity<NotificacionesModel>(e =>
            {
                e.Property(x => x.mensaje).HasMaxLength(255).IsRequired();
                e.Property(x => x.tipo).HasMaxLength(45).IsRequired();

                e.Property(x => x.usuario_id).HasColumnName("usuario_id"); // FK INT a aspnetusers.Id
                e.Property(x => x.id_estado).HasColumnName("id_estado");

                // Sin navegación a Usuario
                e.HasOne<EstadosModel>()
                    .WithMany()
                    .HasForeignKey(x => x.id_estado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<PromocionesModel>().ToTable("promociones").HasKey(x => x.id_promocion);
            b.Entity<PromocionesModel>(e =>
            {
                e.Property(x => x.titulo).HasMaxLength(255).IsRequired();
                e.Property(x => x.descripcion).HasMaxLength(255).IsRequired();

                e.Property(x => x.id_estado).HasColumnName("id_estado");

                e.HasOne<EstadosModel>()
                    .WithMany()
                    .HasForeignKey(x => x.id_estado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<RevisionModel>().ToTable("revision").HasKey(x => x.id_revision);
            b.Entity<RevisionModel>(e =>
            {
                e.Property(x => x.vehiculo_id).HasColumnName("vehiculo_id");
                e.Property(x => x.id_servicio).HasColumnName("id_servicio");
                e.Property(x => x.id_estado).HasColumnName("id_estado");

                e.HasOne<Vehiculo>()
                    .WithMany()
                    .HasForeignKey(x => x.vehiculo_id)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne<ServiciosModel>()
                    .WithMany()
                    .HasForeignKey(x => x.id_servicio)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne<EstadosModel>()
                    .WithMany()
                    .HasForeignKey(x => x.id_estado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<ServiciosRevisionModel>().ToTable("servicios_revision").HasKey(x => x.id_servicio_revision);
            b.Entity<ServiciosRevisionModel>(e =>
            {
                e.Property(x => x.costo_final).HasPrecision(10, 2);

                e.Property(x => x.revision_id).HasColumnName("revision_id");
                e.Property(x => x.servicio_id).HasColumnName("servicio_id");

                e.HasOne<RevisionModel>()
                    .WithMany()
                    .HasForeignKey(x => x.revision_id)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne<ServiciosModel>()
                    .WithMany()
                    .HasForeignKey(x => x.servicio_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<AgendamientoModel>().ToTable("agendamiento").HasKey(x => x.id_agendamiento);
            b.Entity<AgendamientoModel>(e =>
            {
                e.Property(x => x.fecha_agregada).HasMaxLength(150).IsRequired();

                e.Property(x => x.cliente_id).HasColumnName("cliente_id");   // FK INT a aspnetusers.Id
                e.Property(x => x.vehiculo_id).HasColumnName("vehiculo_id");
                e.Property(x => x.id_estado).HasColumnName("id_estado");

                // Sin navegación a Usuario
                e.HasOne<Vehiculo>()
                    .WithMany()
                    .HasForeignKey(x => x.vehiculo_id)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne<EstadosModel>()
                    .WithMany()
                    .HasForeignKey(x => x.id_estado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            b.Entity<ColaboradoresModel>().ToTable("colaboradores").HasKey(x => x.id_colaborador);
            b.Entity<ColaboradoresModel>(e =>
            {
                e.Property(x => x.puesto).HasMaxLength(45).IsRequired();
                e.Property(x => x.salario).HasPrecision(10, 2);

                e.Property(x => x.id_usuario).HasColumnName("id_usuario"); // FK INT a aspnetusers.Id
                // Sin navegación a Usuario
            });

            // =============================
            // SEED (Estados, TipoServicios, Marcas, Servicios)
            // =============================
            b.Entity<EstadosModel>().HasData(
                new EstadosModel { id_estado = 1, nombre = "activo" },
                new EstadosModel { id_estado = 2, nombre = "pendiente" },
                new EstadosModel { id_estado = 3, nombre = "inactivo" },
                new EstadosModel { id_estado = 4, nombre = "Ingresado" },
                new EstadosModel { id_estado = 5, nombre = "En Diagnóstico" },
                new EstadosModel { id_estado = 6, nombre = "Pendiente de aprobación" },
                new EstadosModel { id_estado = 7, nombre = "En mantenimiento" },
                new EstadosModel { id_estado = 8, nombre = "En pruebas" },
                new EstadosModel { id_estado = 9, nombre = "Finalizado" },
                new EstadosModel { id_estado = 10, nombre = "Entregado" },
                new EstadosModel { id_estado = 11, nombre = "Cancelado" }
            );

            b.Entity<TipoServiciosModel>().HasData(
                new TipoServiciosModel { id_tipo_servicio = 1, nombre = "Mantenimiento preventivo", descripcion = "Servicios de mantenimiento periódico" },
                new TipoServiciosModel { id_tipo_servicio = 2, nombre = "Mantenimiento correctivo", descripcion = "Reparaciones por fallas" },
                new TipoServiciosModel { id_tipo_servicio = 3, nombre = "Falla específica", descripcion = "Diagnóstico/Reparación de una falla puntual" }
            );

            b.Entity<MarcasModel>().HasData(
                new MarcasModel { id_marca = 1, nombre = "Jeep" },
                new MarcasModel { id_marca = 2, nombre = "Dodge" },
                new MarcasModel { id_marca = 3, nombre = "Toyota" },
                new MarcasModel { id_marca = 4, nombre = "Nissan" },
                new MarcasModel { id_marca = 5, nombre = "Honda" },
                new MarcasModel { id_marca = 6, nombre = "Mitsubishi" },
                new MarcasModel { id_marca = 7, nombre = "Suzuki" },
                new MarcasModel { id_marca = 8, nombre = "Hyundai" },
                new MarcasModel { id_marca = 9, nombre = "Chevrolet" },
                new MarcasModel { id_marca = 10, nombre = "Chrysler" },
                new MarcasModel { id_marca = 11, nombre = "Daihatsu" },
                new MarcasModel { id_marca = 12, nombre = "RAM" },
                new MarcasModel { id_marca = 13, nombre = "Ford" },
                new MarcasModel { id_marca = 14, nombre = "GMC" },
                new MarcasModel { id_marca = 15, nombre = "Hummer" },
                new MarcasModel { id_marca = 16, nombre = "Isuzu" },
                new MarcasModel { id_marca = 17, nombre = "Kia" },
                new MarcasModel { id_marca = 18, nombre = "Lexus" },
                new MarcasModel { id_marca = 19, nombre = "Mazda" }
            );

            b.Entity<ServiciosModel>().HasData(
                new ServiciosModel { id_servicio = 1, nombre = "Electrónica", descripcion = "Diagnóstico y reparación de sistemas electrónicos", tipo = "Falla específica", precio = 120.00m, tipo_servicio_id = 3 },
                new ServiciosModel { id_servicio = 2, nombre = "Aire acondicionado", descripcion = "Revisión y reparación del sistema de A/C", tipo = "Falla específica", precio = 150.00m, tipo_servicio_id = 3 },
                new ServiciosModel { id_servicio = 3, nombre = "Transmisiones", descripcion = "Reparación de cajas manual/automática", tipo = "Falla específica", precio = 250.00m, tipo_servicio_id = 3 },
                new ServiciosModel { id_servicio = 4, nombre = "Electricidad", descripcion = "Cableado, luces y alternadores", tipo = "Falla específica", precio = 100.00m, tipo_servicio_id = 3 },
                new ServiciosModel { id_servicio = 5, nombre = "Parabrisas", descripcion = "Reemplazo y sellado", tipo = "Falla específica", precio = 180.00m, tipo_servicio_id = 3 },
                new ServiciosModel { id_servicio = 6, nombre = "Tapicería", descripcion = "Reparación y limpieza de interiores", tipo = "Falla específica", precio = 130.00m, tipo_servicio_id = 3 },
                new ServiciosModel { id_servicio = 7, nombre = "Pintura", descripcion = "General o parcial", tipo = "Falla específica", precio = 300.00m, tipo_servicio_id = 3 },
                new ServiciosModel { id_servicio = 8, nombre = "Cambio de aceite", descripcion = "Aceite y filtro", tipo = "Mantenimiento preventivo", precio = 80.00m, tipo_servicio_id = 1 },
                new ServiciosModel { id_servicio = 9, nombre = "Revisión general", descripcion = "Chequeo completo", tipo = "Mantenimiento preventivo", precio = 100.00m, tipo_servicio_id = 1 },
                new ServiciosModel { id_servicio = 10, nombre = "Cambio de frenos", descripcion = "Pastillas y discos", tipo = "Mantenimiento correctivo", precio = 200.00m, tipo_servicio_id = 2 },
                new ServiciosModel { id_servicio = 11, nombre = "Reparación de motor", descripcion = "Fallas graves", tipo = "Mantenimiento correctivo", precio = 500.00m, tipo_servicio_id = 2 }
            );
        }
    }
}
