using BlogApp.Models;
using BlogApp.Repository.Database;
using BlogApp.Repository.Interfaces;
using BlogApp.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Repository.ModelRepositories
{
    public class CommentRepository : Repository<Comment>, IRepository<Comment>
    {
        public CommentRepository(BlogDbContext ctx) : base(ctx)
        {
        }

        public override Comment Read(int id)
        {
            return this.ctx.Comments.FirstOrDefault(t => t.Id == id);
        }

        public override void Update(Comment item)
        {
            var old = Read(item.Id);
            foreach (var prop in old.GetType().GetProperties())
            {
                prop.SetValue(item, prop.GetValue(item));
            }
            ctx.SaveChanges();
        }
    }
}
