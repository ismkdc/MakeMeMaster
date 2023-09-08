using MakeMeFaster.DataAPI;
using MakeMeFaster.DataAPI.Data;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.TypeInfoResolver = MakeMeFasterJsonSerializerContext.Default
);

builder.Services.AddDbContextPool<MakeMeFasterContext>(
    options =>
        options.UseNpgsql(
            "Host=127.0.0.1;Port=5432;Username=usr;Password=passwd;Database=db"
        )
);

var app = builder.Build();

app.MapGet("/products", (MakeMeFasterContext context) => MakeMeFasterContext.GetAllProductsAsync(context));
app.MapGet("/categories/{id:int}",
    (MakeMeFasterContext context, int id) => MakeMeFasterContext.GetCategoryByIdAsync(context, id));

app.Run();