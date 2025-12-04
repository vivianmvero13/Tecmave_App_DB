using Front.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Tecmave.Api.Services;
using Tecmave.Front.Models;


var builder = WebApplication.CreateBuilder(args);

// ======================
//       RAZOR PAGES
// ======================
builder.Services.AddRazorPages();



// ======================
//     HTTP CLIENT API
// ======================
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
if (string.IsNullOrWhiteSpace(apiBaseUrl))
{
    throw new InvalidOperationException("No se encontró la configuración 'ApiBaseUrl' en appsettings.");
}

// Cliente tipado para la API
builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Cliente genérico si ocupás otros usos
builder.Services.AddHttpClient();

// ======================
//     SMTP / EMAIL
// ======================
// IEmailSender viene de Microsoft.AspNetCore.Identity.UI.Services
builder.Services.AddTransient<Tecmave.Api.Services.IEmailSender, SmtpEmailSender>();

// ======================
//    CONEXIÓN A MySQL
// ======================
var cs = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<MyIdentityDBContext>(opt =>
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs)));

// ======================
//        IDENTITY
// ======================
builder.Services.AddIdentity<Usuario, IdentityRole<int>>(
    options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;

        options.SignIn.RequireConfirmedEmail = false;

        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<MyIdentityDBContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// ======================
//         CORS
// (solo afecta si otro origen consume el FRONT directamente)
// ======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// ======================
//      PIPELINE APP
// ======================
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowLocalhost");

app.MapRazorPages();

app.Run();