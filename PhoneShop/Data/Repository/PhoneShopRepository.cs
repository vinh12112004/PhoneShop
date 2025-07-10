
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PhoneShop.Data.Repository
{
    public class PhoneShopRepository<T> : IPhoneShopRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public PhoneShopRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            await _dbSet.AddAsync(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }
        public async Task<bool> DeleteAsync(T dbRecord)
        {

            _dbSet.Remove(dbRecord);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useAsNoTracking = false)
        {
            if (useAsNoTracking)
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }
        public async Task<List<T>> GetAllAsyncByFilter(Expression<Func<T, bool>> filter, bool useAsNoTracking = false)
        {
            if (useAsNoTracking)
                return await _dbSet.AsNoTracking().Where(filter).ToListAsync();
            else
                return await _dbSet.Where(filter).ToListAsync();
        }
        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbSet.Update(dbRecord);


            await _dbContext.SaveChangesAsync();

            return dbRecord;
        }
    }
}
