using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Models
{
    public class OrderAddDto
    {
        [Required]
        public List<OrderItemAddDto> OrderItems { get; set; }
    }
}
