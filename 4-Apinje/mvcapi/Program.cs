using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvcapi.Context;

var builder = WebApplication.CreateBuilder(args);

var corsName = "AllowedOrigins";
var allowedOrigins = Environment.GetEnvironmentVariable("OriginsCors")?.Split(',');
Console.WriteLine($"[+] Origins: {allowedOrigins}");

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

/*
app.Use(async (context, next) =>
{
    var origin = context.Request.Headers["Origin"].ToString();
    //Con esto veo el origin de la solicitud
    Console.WriteLine($"[+] Origen: {origin};");

    if (allowedOrigins.Contains(origin))
    {
        context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
        context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
    }

    // Manejar solicitudes OPTIONS
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = StatusCodes.Status204NoContent;
        return;
    }

    await next.Invoke();
});
*/

app.MapControllers();
app.Run();
