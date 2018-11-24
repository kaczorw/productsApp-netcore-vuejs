﻿using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ProductsDbContext _context;

        public OrderRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
              .Include(x => x.AppUser)
              .Include(x => x.OrderItems)
              .ThenInclude(x => x.Product)
              .OrderByDescending(x => x.DateCreated)
              .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
              .Include(x => x.OrderItems)
              .ThenInclude(a => a.Product)
              .FirstOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string id)
        {
            return await _context.Orders
               .Include(x => x.AppUser)
               .Include(x => x.OrderItems)
               .ThenInclude(x => x.Product)
               .Where(x => x.AppUserId == id)
               .OrderByDescending(x => x.DateCreated)
               .ToListAsync();
        }

        public async Task RemoveAsync(Order order)
        {
            _context.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
