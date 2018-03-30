using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using User.API.Data;
using User.API.Models;

namespace User.API.Repositories
{
    public class AppUserRepository : IRepository<AppUser, int>
    {
        private readonly MyContext _context;

        public AppUserRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(AppUser entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var appUser = _context.AppUsers.FirstOrDefault(a => a.Id == id);
            if (appUser != null)
            {
                await Task.Run(() => { _context.AppUsers.Remove(appUser); });
            }
        }

        public async Task<IEnumerable<AppUser>> FindAsync(Expression<Func<AppUser, bool>> expression=null)
        {
            return await Task.FromResult(GetAllList().ToList());
        }

        public IQueryable<AppUser> GetAllList(Expression<Func<AppUser, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _context.AppUsers;
            }
            return _context.AppUsers.Where(predicate);
        }


        public async Task<AppUser> FindByIdAsync(int id)
        {
            var appUser = _context.AppUsers.FirstOrDefault(a => a.Id == id);
            return await Task.FromResult(appUser);
        }

        public async Task UpdateAsync(AppUser entity)
        {
            var oriUser = _context.AppUsers.Single(x => x.Id == entity.Id);
            _context.Entry(oriUser).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<AppUser> SinleAsync(Expression<Func<AppUser, bool>> expression = null)
        {
            var appUser = _context.AppUsers.FirstOrDefault(expression);
            return await Task.FromResult(appUser);
        }
    }
}
