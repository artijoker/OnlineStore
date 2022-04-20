using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HttpModels
{
    public class OrderItem : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
        public Product? Product { get; set; }

    }
}
