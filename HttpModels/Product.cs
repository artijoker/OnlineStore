namespace HttpModels
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? UrlImage { get; set; }

        public Category? Category { get; set; }
       
    }
}
