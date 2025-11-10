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
builder.Services.AddScoped<RevisionService>();
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
/*
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();

    string[] roles = { "Administrador", "Usuarios" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new AppRole { Name = role });
    }

    var adminEmail = "admin@tecmave.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);
    if (admin == null)
    {
        admin = new Usuario { UserName = "admin", Nombre = "Admin", Apellidos = "Principal", Email = adminEmail };
        await userManager.CreateAsync(admin, "Admin1234");
        await userManager.AddToRoleAsync(admin, "Administrador");
    }

    var userEmail = "usuario@tecmave.com";
    var normalUser = await userManager.FindByEmailAsync(userEmail);
    if (normalUser == null)
    {
        normalUser = new Usuario
        {
            UserName = "usuario",
            Nombre = "Usuario",
            Apellidos = "Prueba",
            Email = userEmail
        };

        await userManager.CreateAsync(normalUser, "Usuario1234");
        await userManager.AddToRoleAsync(normalUser, "Usuarios");
    }
}
*/

app.Run();
