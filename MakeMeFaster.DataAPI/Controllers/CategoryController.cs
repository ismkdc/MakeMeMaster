using MakeMeFaster.DataAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace MakeMeFaster.DataAPI.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController : ControllerBase
{
    private readonly MakeMeFasterContext _context;

    public CategoryController(MakeMeFasterContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<Category> Get([FromQuery] int id)
    {
        return _context
            .Categories
            .ToList()
            .Where(productCategory => productCategory.Id == id);
    }
}