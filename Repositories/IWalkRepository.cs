﻿using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Repositories;

public interface IWalkRepository
{
    Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filter = null);
    Task<Walk?> GetByIdAsync(Guid id);
    Task<Walk> CreateAsync(Walk walk);
    Task<Walk?> UpdateAsync(Guid id, Walk walk);
    Task<Walk?> DeleteAsync(Guid id);
}
