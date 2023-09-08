using System.Net;
using System.Text.Json;
using MakeMeFaster;
using MakeMeFaster.Data;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.TypeInfoResolver = MakeMeFasterJsonSerializerContext.Default
);

builder.Services.AddHttpClient("MakeMeFaster");

var app = builder.Build();

var jsonSerializerOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    TypeInfoResolver = MakeMeFasterJsonSerializerContext.Default
};

app.MapGet("/products", (IHttpClientFactory httpClientFactory) =>
    {
        var httpClient = httpClientFactory.CreateClient("MakeMeFaster");
        return Handle();

        async IAsyncEnumerable<ProductResponse> Handle()
        {
            var productsResponse =
                await httpClient.GetAsync("http://localhost:5096/products", HttpCompletionOption.ResponseHeadersRead);
            var productsStream = await productsResponse.Content.ReadAsStreamAsync();

            await foreach (var product in JsonSerializer.DeserializeAsyncEnumerable<Product>(productsStream,
                               jsonSerializerOptions))
            {
                var category =
                    await httpClient.GetFromJsonAsync<Category>(
                        $"http://localhost:5096/categories/{product.CategoryId}",
                        jsonSerializerOptions);

                yield return new ProductResponse
                {
                    Product = product,
                    Category = category
                };
            }
        }
    }
);

app.Run();