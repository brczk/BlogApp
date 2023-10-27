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
            if(item.Content.Length < 5)
            {
                throw new ArgumentException("Too short content");
            }

            if (item.Id < 1)
            {
                throw new ArgumentException("Invalid ID");
            }
            repo.Create(item);
        }

        public void Delete(int id)
        {
            this.Read(id);
            repo.Delete(id);
        }

        public Post Read(int id)
        {
            var post = repo.Read(id);
            if (post == null)
            {
                throw new NullReferenceException("Non-existent post");
            }
            return post;
        }

        public IQueryable<Post> ReadAll()
        {
            return repo.ReadAll();
        }

        public void Update(Post item)
        {
            this.Read(item.Id);
            if (item.Content.Length < 5)
            {
                throw new ArgumentException("Too short content");
            }
            repo.Update(item);
        }
    }
}
