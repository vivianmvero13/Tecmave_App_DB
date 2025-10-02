using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cs = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs)));

builder.Services
    .AddIdentityCore<Usuario>(o =>
    {
        o.User.RequireUniqueEmail = false;
        o.Password.RequireDigit = true;
        o.Password.RequiredLength = 6;
        o.Password.RequireLowercase = true;
        o.Password.RequireUppercase = true;
        o.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermirFrontend",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<UserAdminService>();
builder.Services.AddScoped<RolesService>();

builder.Services.AddScoped<VehiculosService>();
builder.Services.AddScoped<TipoServiciosService>();
builder.Services.AddScoped<ServiciosService>();
builder.Services.AddScoped<ServiciosRevisionModel>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<ResenasService>();
builder.Services.AddScoped<PromocionesService>();
builder.Services.AddScoped<NotificacionesService>();
builder.Services.AddScoped<ModelosService>();
builder.Services.AddScoped<MarcasService>();
builder.Services.AddScoped<FacturasService>();
builder.Services.AddScoped<EstadosService>();
builder.Services.AddScoped<ColaboradoresService>();
builder.Services.AddScoped<AgendamientoService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("PermirFrontend");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
