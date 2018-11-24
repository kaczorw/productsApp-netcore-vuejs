using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext _context;

        public ProductRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            return await _context.Products
                .Include(x => x.Category)
                .Where(x => x.Category.Name.Equals(category))
                .ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetByIdAsync(int id, bool includeCategory = true)
        {
            if (includeCategory)
            {
                return await _context.Products
                .Include(x => x.Category)
                .Where(x => x.ProductId == id)
                .FirstOrDefaultAsync();
            }

            return await _context.Products
                .Where(x => x.ProductId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(bool includeHiddenProducts)
        {
            if (includeHiddenProducts)
            {
                return await _context.Products.Include(x => x.Category).ToListAsync();

            }
            return await _context.Products.Include(x => x.Category).Where(x => x.IsHidden == false).ToListAsync();
        }
    }
}
