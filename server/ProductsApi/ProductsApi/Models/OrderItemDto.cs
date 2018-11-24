using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Models
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public int Weight { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Price { get; set; }
    }
}
