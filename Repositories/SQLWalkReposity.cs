using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Repositories
{
    public class SQLWalkRepository(NZWalksDbContext dbContext) : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext = dbContext;

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(r => r.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            _dbContext.Remove(existingWalk);
            await _dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filter = null)
        {
            var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filter))
            {
                switch (filterOn.ToLower())
                {
                    case "name":
                        walks = walks.Where(w => w.Name.Contains(filter));
                        break;
                    case "description":
                        walks = walks.Where(w => w.Description.Contains(filter));
                        break;
                    case "length":
                        if (double.TryParse(filter, out double length))
                        {
                            walks = walks.Where(w => w.Length == length);
                        }
                        break;
                    default:
                        break;
                }
            }

            return await walks.ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = _dbContext.Walks.FirstOrDefault(r => r.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.Description = walk.Description;
            existingWalk.ImageUrl = walk.ImageUrl;

            await _dbContext.SaveChangesAsync();

            return existingWalk;
        }

    }
}