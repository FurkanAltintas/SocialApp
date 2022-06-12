using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;

namespace ServerApp.Repositories
{
    public class Repository : IRepository
    {
        private readonly SocialContext _context;
        
        public Repository(SocialContext context)
        {
            _context = context;
        }

        public async Task AddAsync<T>(T entity) where T : class
            => await _context.AddAsync(entity);

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> filter) where T : class
            => await _context.Set<T>().AnyAsync(filter);

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>>? filter = null) where T : class
            => await _context.Set<T>().CountAsync(filter);

        public void Delete<T>(T entity) where T : class
            => _context.Remove(entity);

        public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> filter = null) where T : class
        {
            var query = Query<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.AsEnumerable();
        }

        public async Task<T> GetAsync<T>(int id) where T : class
            => await _context.Set<T>().FindAsync(id);

        public T GetAsync<T>(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            var query = Query<T>();

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties.Any())
                foreach (var includeProperty in includeProperties)
                    query = query.Include(includeProperty);

            return query.SingleOrDefault();
        }

        public async Task<IEnumerable<int>> GetFollows(int userId, bool isFollowings)
        {
            var user = await _context.Users.Include(u => u.Followers).Include(u => u.Followings).FirstOrDefaultAsync(u => u.Id == userId);

            if (isFollowings)
            {
                return user.Followers
                    .Where(u => u.FollowerId == userId)
                    .Select(u => u.UserId);
            }
            else
            {
                return user.Followings
                    .Where(u => u.UserId == userId)
                    .Select(u => u.FollowerId);
            }
        }

        public IQueryable<T> Query<T>() where T : class
            => _context.Set<T>().AsQueryable();

        public async Task<bool> SaveChanges()
            =>  await _context.SaveChangesAsync() > 0;
    }
}