using EcommerceProject.Context;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Repositories
{
    public class GenericRepository<TEntity>(AppDbContext _dbContext) where TEntity : class
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();

        }
    }
}
