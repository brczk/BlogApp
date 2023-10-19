using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Logic.Interfaces
{
    public interface ICommentLogic
    {
        void Create(Comment item);
        void Delete(int id);
        Comment Read(int id);
        IQueryable<Comment> ReadAll();
        void Update(Comment item);
    }
}
