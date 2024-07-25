using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvcapi.Context;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS
var corsName = "AllowAllOrigins";
var allowedOrigins = builder.Configuration.GetSection("OriginsCors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsName, policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Registro de servicios
builder.Services.AddControllers();
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();

// Registro del MD5PasswordHasher
builder.Services.AddTransient<IPasswordHasher<IdentityUser>, MD5PasswordHasher<IdentityUser>>();

// Configuración de la autenticación con cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/api/Auth/login";
    options.AccessDeniedPath = "/api/Auth/access-denied";
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
