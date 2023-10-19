using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Logic.Interfaces
{
    public interface IPostLogic
    {
        void Create(Post item);
        void Delete(int id);
        Post Read(int id);
        IQueryable<Post> ReadAll();
        void Update(Post item);
    }
}
