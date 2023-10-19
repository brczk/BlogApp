using BlogApp.Logic.Interfaces;
using BlogApp.Models;
using BlogApp.Repository.GenericRepository;
using BlogApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Logic.Classes
{
    public class BlogLogic : IBlogLogic
    {
        IRepository<Blog> repo;

        public BlogLogic(IRepository<Blog> repo)
        {
            this.repo = repo;
        }

        public void Create(Blog item)
        {
            if (item.URL.Length < 3)
            {
                throw new ArgumentException("Short title");
            }
            repo.Create(item);
        }

        public void Delete(int id)
        {
            repo.Delete(id);
        }

        public Blog Read(int id)
        {
            var movie = repo.Read(id);
            if (movie == null)
            {
                throw new ArgumentException("Non-existent movie");
            }
            return movie;
        }

        public IQueryable<Blog> ReadAll()
        {
            return repo.ReadAll();
        }

        public void Update(Blog item)
        {
            repo.Update(item);
        }
    }
}
