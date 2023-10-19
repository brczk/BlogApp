using BlogApp.Models;
using System;
using System.Linq;
using System.Numerics;

namespace BlogApp.Logic.Interfaces
{
    public interface IBlogLogic
    {
        void Create(Blog item);
        void Delete(int id);
        Blog Read(int id);
        IQueryable<Blog> ReadAll();
        void Update(Blog item);
    }
}
