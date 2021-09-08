using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Library_Management_System.Entity;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        bool Delete(T entiity);
        bool UpsertAsync(T entity);
        Task SaveChanges();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return true;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public bool Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return true;
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public bool UpsertAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return true;
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }


    }
}
