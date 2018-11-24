using ProductsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync(bool includeHiddenProducts);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(string category);
        Task AddAsync(ProductAddDto productAddDto);
        Task UpdateAsync(ProductUpdateDto productUpdateDto);
    }
}
