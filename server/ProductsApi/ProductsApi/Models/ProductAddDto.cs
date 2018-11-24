using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Models
{
    public class ProductAddDto
    {
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Description { get; set; }
        [Required]
        [Range(0, 3000)]
        public int Weight { get; set; }
        [Required]
        [Range(0, 1000)]
        [RegularExpression(@"\d+(\,\d{1,2})?")]
        public decimal Price { get; set; }
        [Required]
        public bool IsHidden { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
