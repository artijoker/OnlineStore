using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HttpModels
{
    public class CartItem : IEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid CartId { get; set; }

        [JsonIgnore]
        public Cart? Cart { get; set; }
        public Product? Product { get; set; }

    }
}
