using System.Linq.Expressions;

namespace PhoneShop.Data.Repository
{
    public interface IPhoneShopRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useAsNoTracking = false);
        Task<T> CreateAsync(T dbRecord);
        Task<T> UpdateAsync(T dbRecord);
        Task<bool> DeleteAsync(T dbRecord);
        Task<List<T>> GetAllAsyncByFilter(Expression<Func<T, bool>> filter, bool useAsNoTracking = false);
        Task<T> GetAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IQueryable<T>> include,
            bool useAsNoTracking = false);    }
}
