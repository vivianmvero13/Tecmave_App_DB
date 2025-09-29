using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<AppDbContext>(o => o.UseMySql(conn, ServerVersion.AutoDetect(conn)));

builder.Services.AddIdentity<Usuario, IdentityRole<int>>(o =>
{
    o.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<AgendamientoService>();
builder.Services.AddScoped<TipoServiciosService>();
builder.Services.AddScoped<ServiciosService>();
builder.Services.AddScoped<RevisionService>();
builder.Services.AddScoped<EstadosService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<MarcasService>();
builder.Services.AddScoped<ModelosService>();
builder.Services.AddScoped<FacturasService>();
builder.Services.AddScoped<DetalleFacturaService>();
builder.Services.AddScoped<ResenasService>();
builder.Services.AddScoped<VehiculosService>();
builder.Services.AddScoped<PromocionesService>();
builder.Services.AddScoped<NotificacionesService>();
builder.Services.AddScoped<ColaboradoresService>();
builder.Services.AddScoped<UserAdminService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
