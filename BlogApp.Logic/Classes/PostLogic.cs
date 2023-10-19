using BlogApp.Logic.Interfaces;
using BlogApp.Models;
using BlogApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Logic.Classes
{
    public class PostLogic : IPostLogic
    {
        IRepository<Post> repo;

        public PostLogic(IRepository<Post> repo)
        {
            this.repo = repo;
        }

        public void Create(Post item)
        {
            repo.Create(item);
        }

        public void Delete(int id)
        {
            repo.Delete(id);
        }

        public Post Read(int id)
        {
            return repo.Read(id);
        }

        public IQueryable<Post> ReadAll()
        {
            return repo.ReadAll();
        }

        public void Update(Post item)
        {
            repo.Update(item);
        }
    }
}
