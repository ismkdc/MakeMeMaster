using System.ComponentModel.DataAnnotations.Schema;

namespace MakeMeFaster.DataAPI.Data;

[Table("productcategory")]
public class Category
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
}