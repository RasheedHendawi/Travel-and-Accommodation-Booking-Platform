﻿using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string name);
    }
}
