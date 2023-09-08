using Microsoft.EntityFrameworkCore;

namespace MakeMeFaster.DataAPI.Data;

public class MakeMeFasterContext: DbContext
{
    public MakeMeFasterContext(DbContextOptions<MakeMeFasterContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    public static readonly Func<MakeMeFasterContext, IAsyncEnumerable<Product>> GetAllProductsAsync
        = EF.CompileAsyncQuery((MakeMeFasterContext context)
            => context.Products);
    
    public static readonly Func<MakeMeFasterContext, int, Task<Category>> GetCategoryByIdAsync
        = EF.CompileAsyncQuery((MakeMeFasterContext context, int id)
            => context.Categories.First(category => category.Id == id));
}