using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Repository.Database;
using BlogApp.Repository.Interfaces;

namespace BlogApp.Repository.GenericRepository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected BlogDbContext ctx;
        public Repository(BlogDbContext ctx)
        {
            this.ctx = ctx;
        }
        public void Create(T item)
        {
            ctx.Set<T>().Add(item);
            ctx.SaveChanges();
        }
        public IQueryable<T> ReadAll()
        {
            return ctx.Set<T>();
        }
        public void Delete(int id)
        {
            ctx.Set<T>().Remove(Read(id));
            ctx.SaveChanges();
        }
        public abstract T Read(int id);
        public abstract void Update(T item);
    }
}
