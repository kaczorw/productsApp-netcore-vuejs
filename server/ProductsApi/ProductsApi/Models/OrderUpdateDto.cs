using ProductsApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Models
{
    public class OrderUpdateDto
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public Status Status { get; set; }
    }
}
