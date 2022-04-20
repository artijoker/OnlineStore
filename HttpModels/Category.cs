using System.Text.Json.Serialization;

namespace HttpModels
{
    public class Category : IEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}