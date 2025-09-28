using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Data;
using Tecmave.Api.Models;
using Tecmave.Api.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

var _connectionStrings = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(_connectionStrings, ServerVersion.AutoDetect(_connectionStrings))
);

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




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

builder.Services
    .AddIdentity<Usuario, RolesModel>(o => { o.SignIn.RequireConfirmedAccount = false; })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
