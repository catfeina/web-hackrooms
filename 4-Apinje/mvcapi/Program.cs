using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvcapi.Context;

var builder = WebApplication.CreateBuilder(args);

var corsName = "AllowedOrigins";
var allowedOrigins = Environment.GetEnvironmentVariable("OriginsCors")?.Split(',');

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

builder.Services.AddControllers();
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IPasswordHasher<IdentityUser>, MD5PasswordHasher<IdentityUser>>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = false; //false para que sea accesible por JS
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; //.Always para https
    options.Cookie.SameSite = SameSiteMode.Lax; //.None para no permitir cookies en http
    options.LoginPath = "/api/Auth/login";
    options.AccessDeniedPath = "/api/Auth/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromHours(1); // Duración de la cookie configurada a 1 hora
    //options.SlidingExpiration = true; // Renovar la cookie si el usuario está activo
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
