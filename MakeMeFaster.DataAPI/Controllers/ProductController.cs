using MakeMeFaster.DataAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace MakeMeFaster.DataAPI.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly MakeMeFasterContext _context;

    public ProductController(MakeMeFasterContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<Product> Get()
    {
        return _context
            .Products
            .ToList();
    }
}