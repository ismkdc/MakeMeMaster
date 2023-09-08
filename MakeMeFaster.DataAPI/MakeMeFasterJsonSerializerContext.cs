using System.Text.Json.Serialization;
using MakeMeFaster.DataAPI.Data;

namespace MakeMeFaster.DataAPI;

[JsonSerializable(typeof(IAsyncEnumerable<Product>))]
[JsonSerializable(typeof(Category))]
public partial class MakeMeFasterJsonSerializerContext : JsonSerializerContext
{
    
}