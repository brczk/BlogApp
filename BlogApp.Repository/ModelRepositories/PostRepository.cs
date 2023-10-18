using BlogApp.Models;
using BlogApp.Repository.Database;
using BlogApp.Repository.Interfaces;
using MovieDbApp.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Repository.ModelRepositories
{
    public class PostRepository : Repository<Post>, IRepository<Post>
    {
        public PostRepository(BlogDbContext ctx) : base(ctx)
        {
        }

        public override Post Read(int id)
        {
            return this.ctx.Posts.FirstOrDefault(t => t.Id == id);
        }

        public override void Update(Post item)
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
