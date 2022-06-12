using System.Linq.Expressions;

namespace ServerApp.Repositories
{
    public interface IRepository
    {
        IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> filter=null) where T : class;

        Task<T> GetAsync<T>(int id) where T : class;

        T GetAsync<T>(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties) where T : class;

        IQueryable<T> Query<T>() where T : class;

        Task AddAsync<T>(T entity) where T : class;
        
        void Delete<T>(T entity) where T : class;

        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> filter) where T : class;

        Task<int> CountAsync<T>(Expression<Func<T, bool>>? filter =null) where T : class;

        Task<IEnumerable<int>> GetFollows(int userId, bool isFollowings);

        Task<bool> SaveChanges();
    }
}