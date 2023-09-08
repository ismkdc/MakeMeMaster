using System.Text.Json.Serialization;
using MakeMeFaster.Data;

namespace MakeMeFaster;

[JsonSerializable(typeof(IAsyncEnumerable<ProductResponse>))]
[JsonSerializable(typeof(IAsyncEnumerable<Product>))]
[JsonSerializable(typeof(Category))]
public partial class MakeMeFasterJsonSerializerContext : JsonSerializerContext
{
    
}