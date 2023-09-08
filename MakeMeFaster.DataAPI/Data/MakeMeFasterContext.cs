using Microsoft.EntityFrameworkCore;

namespace MakeMeFaster.DataAPI.Data;

public class MakeMeFasterContext: DbContext
{
    public MakeMeFasterContext(DbContextOptions<MakeMeFasterContext> options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}