var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
var _ng = builder.Configuration.GetValue<string>("angular");
var _origins = "AllowAll";
builder.Services.AddCors(options => {
    options.AddPolicy(name: _origins, policy => {
        /*
        policy.WithOrigins(_ng, "http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
        */
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplicar la política de CORS
app.UseCors(_origins);

//app.UseHttpsRedirection();

var _msg = builder.Configuration.GetValue<string>("msg");
app.MapGet("/test", () => Results.Json(new {message = _msg}));

app.MapGet("/ip", () => Results.Json(new {ip = "189.192.1.125"}));
app.MapGet("/nothinghere/{flag110}/{flag6}/{creature}", (string flag110, string flag6, string creature) =>
    ResponseHelper.CreateResponse("WW91JTI3cmUlMjBhJTIwZHVjayUyMSUyMFlvdXIlMjBmbGFnJTNBJTIwZmNkZWNhNjg0NzVhNzc4MmE3ZmFiMDMwYzE2ZjgwZTg=")
).AddEndpointFilter<FlagFilter>();

var beers = new Beer[] {
    new Beer("beer1", "brand1"),
    new Beer("beer2", "brand2"),
    new Beer("beer3", "brand3"),
    new Beer("beer4", "brand4"),
    new Beer("beer5", "brand5")
};

app.MapGet("/beers/{quantity}", (int quantity) => {
    return beers.Take(quantity);
}).AddEndpointFilter<BeerFilter>();

app.Run();

internal record Beer(string name, string brand);

public class BeerFilter : IEndpointFilter {
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    ) {
        int quantity = context.GetArgument<int>(0);

        if (quantity <= 0)
            return Results.Problem("No se permiten menos de 1 unidad.");

        if (quantity >= 6)
            return Results.Problem("No se permiten más de 5 unidades.");

        return await next(context);
    }
}

public class FlagFilter : IEndpointFilter {
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    ) {
        string flag110 = context.GetArgument<string>(0);
        string flag6 = context.GetArgument<string>(1);
        string creature = context.GetArgument<string>(2);

        if (!flag110.Equals("09af8935b75fd7bd40d88d6c8645c99b"))
            return ResponseHelper.CreateResponse("Flag 110 incorrecta");

        if (!flag6.Equals("07d88f3282aca768e56e76d83de4049e"))
            return ResponseHelper.CreateResponse("Flag 6 incorrecta");
        
        if (!creature.Equals("insano"))
            return ResponseHelper.CreateResponse("Criatura incorrecta");
        
        return await next(context);
    }
}

public static class ResponseHelper {
    public static IResult CreateResponse(string message) {
        return Results.Json(new { msg = message });
    }
}
