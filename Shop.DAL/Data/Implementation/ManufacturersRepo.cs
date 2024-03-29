﻿using Microsoft.EntityFrameworkCore;
using Shop.DAL.Data.Interfaces;
using Shop.DAL.Models;

namespace Shop.DAL.Data.Implementation
{
    public class ManufacturersRepo : IManufacturersRepo
    {
        private readonly AppDbContext _dbContext;
        public ManufacturersRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddManufacturer(Manufacturer manufacturer)
        {
            await _dbContext.Manufacturers.AddAsync(manufacturer);
        }

        public void DeleteManufacturer(Manufacturer manufacturer)
        {
            _dbContext.Manufacturers.Remove(manufacturer);
        }

        public async Task<IEnumerable<Manufacturer>> GetAllManufacturers()
        {
            return await _dbContext.Manufacturers.Include(m => m.Products).ThenInclude(p => p.Category)
                                                 .Include(m => m.Image)
                                                 .ToListAsync();
        }

        public async Task<Manufacturer> GetManufacturerById(int id)
        {
            return await _dbContext.Manufacturers.Include(m => m.Products).ThenInclude(p => p.Category)
                .Include(m => m.Image)
                                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Manufacturer> GetManufacturerByName(string name)
        {
            return await _dbContext.Manufacturers.Include(m => m.Products).ThenInclude(p => p.Category)
                .Include(m => m.Image)
                                                 .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
