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
    public class CommentLogic : ICommentLogic
    {
        IRepository<Comment> repo;

        public CommentLogic(IRepository<Comment> repo)
        {
            this.repo = repo;
        }

        public void Create(Comment item)
        {
            repo.Create(item);
        }

        public void Delete(int id)
        {
            repo.Delete(id);
        }

        public Comment Read(int id)
        {
            return repo.Read(id);
        }

        public IQueryable<Comment> ReadAll()
        {
            return repo.ReadAll();
        }

        public void Update(Comment item)
        {
            repo.Update(item);
        }
    }
}
