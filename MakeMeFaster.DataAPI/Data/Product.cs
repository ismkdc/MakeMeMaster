using System.ComponentModel.DataAnnotations.Schema;

namespace MakeMeFaster.DataAPI.Data;

[Table("products")]
public class Product
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("description")]
    public string Description { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("stock_quantity")]
    public int StockQuantity { get; set; }
    [Column("category_id")]
    public int CategoryId { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}