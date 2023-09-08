using System.Text.Json;
using MakeMeFaster.Data;
using Microsoft.AspNetCore.Mvc;

namespace MakeMeFaster.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    [HttpGet]
    public IEnumerable<ProductResponse> Get()
    {
        var httpClient = new HttpClient();
        var productResponse = httpClient.GetAsync("http://localhost:5096/products").Result;
        var productContent = productResponse.Content.ReadAsStringAsync().Result;
        
        var products = JsonSerializer.Deserialize<List<Product>>(productContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var productResponses = new List<ProductResponse>();
        foreach (var product in products)
        {
            var categoryResponse = httpClient.GetAsync($"http://localhost:5096/categories?id={product.CategoryId}").Result;
            var categoryContent = categoryResponse.Content.ReadAsStringAsync().Result;
        
            var category = JsonSerializer.Deserialize<IEnumerable<Category>>(categoryContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }).FirstOrDefault();
            
            productResponses.Add(new ProductResponse
            {
                Product = product,
                Category = category
            });
        }

        return productResponses;
    }
}