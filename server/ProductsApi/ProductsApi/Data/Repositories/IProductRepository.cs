﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Data.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id, bool includeCategory = true);
        Task<IEnumerable<Product>> GetAllAsync(bool includeHiddenProducts);
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);

    }
}
