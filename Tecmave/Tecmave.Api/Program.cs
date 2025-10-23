using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;                
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CONEXIÓN A MySQL ---
var cs = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs)));

// --- IDENTITY CONFIG ---
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

// --- CORS (Frontend permitido) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermirFrontend", policy => policy
        .WithOrigins(
            "https://localhost:7190",
            "http://localhost:5173",
            "https://localhost:5173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
    );
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<UserAdminService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<VehiculosService>();
builder.Services.AddScoped<TipoServiciosService>();
builder.Services.AddScoped<ServiciosService>();
builder.Services.AddScoped<ResenasService>();
builder.Services.AddScoped<PromocionesService>();
builder.Services.AddScoped<NotificacionesService>();
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
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
