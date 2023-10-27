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
            if (item.Content.Length < 5)
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

        public Comment Read(int id)
        {
            var comment = repo.Read(id);
            if (comment == null)
            {
                throw new NullReferenceException("Non-existent comment");
            }
            return comment;
        }

        public IQueryable<Comment> ReadAll()
        {
            return repo.ReadAll();
        }

        public void Update(Comment item)
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
