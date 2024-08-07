using System.Security.Claims;
using apirest.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
const string corsName = "AllowedOrigings";
var originsAllowed = builder.Configuration.GetSection("OriginsCors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsName, policy =>
    {
        policy.WithOrigins(originsAllowed)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<RestContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "Patoken";
    options.Cookie.HttpOnly = false;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.LoginPath = "/api/User/Login";
    options.AccessDeniedPath = "/api/User/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    /*
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
    */
    options.Events.OnSigningIn = async context =>
   {
       var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
       var user = await userManager.GetUserAsync(context.Principal);
       if (user != null)
       {
           var roles = await userManager.GetRolesAsync(user);
           var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
           foreach (var role in roles)
           {
               claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
           }
       }
   };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RestContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
