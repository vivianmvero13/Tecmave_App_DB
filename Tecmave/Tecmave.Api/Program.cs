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
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs),
        b => b.MigrationsHistoryTable("__EFMigrationsHistory_App")));




// --- IDENTITY CONFIG ---
builder.Services
    .AddIdentity<Usuario, AppRole>(options =>
    {
        options.User.RequireUniqueEmail = false;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// --- CORS (Frontend permitido) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermirFrontend", policy => policy
        .WithOrigins(
            "http://localhost:7190",
            "http://localhost:5173",
            "http://localhost:5173"
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
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<NotificacionesService>();
builder.Services.AddScoped<RevisionService>();
builder.Services.AddScoped<MarcasService>();
builder.Services.AddScoped<FacturasService>();
builder.Services.AddScoped<EstadosService>();
builder.Services.AddScoped<ColaboradoresService>();
builder.Services.AddScoped<PlanillasService>();
builder.Services.AddScoped<AgendamientoService>();
builder.Services.AddScoped<AgendamientoService>();
builder.Services.AddHostedService<RecordatorioService>();
builder.Services.AddScoped<MantenimientoService>();
builder.Services.AddScoped<RevisionPertenenciasService>();
builder.Services.AddScoped<RevisionTrabajosService>();
builder.WebHost.UseWebRoot("wwwroot");


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("PermirFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
