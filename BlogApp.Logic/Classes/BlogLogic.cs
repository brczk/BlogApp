using BlogApp.Logic.Interfaces;
using BlogApp.Models;
using BlogApp.Repository.GenericRepository;
using BlogApp.Repository.Interfaces;
using System;
using System.Collections;
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

        #region CRUD

        public void Create(Blog item)
        {
            if (item.BlogName.Length < 5)
            {
                throw new ArgumentException("Short blog name");
            }

            if (item.URL.Split(':')[0] != "https")
            {
                throw new ArgumentException("Insecure protocol");
            }

            if (item.Id < 0)
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

        public Blog Read(int id)
        {
            var blog = repo.Read(id);
            if (blog == null)
            {
                throw new ArgumentException("Non-existent blog");
            }
            return blog;
        }

        public IQueryable<Blog> ReadAll()
        {
            return repo.ReadAll();
        }

        public void Update(Blog item)
        {
            repo.Update(item);
        }

        #endregion
        #region Non-CRUD
        public IEnumerable<CategoryCountInfo> PostsCountInCategories()
        {
            var blogs = repo.ReadAll().ToList();
            List<Post> posts = new List<Post>();
            blogs.ForEach(b => posts.AddRange(b.Posts));
 
            return from post in posts
                   group post by post.Category into categories
                   select new CategoryCountInfo()
                   {
                       CategoryName = categories.Key,
                       CategoryCount = categories.Count()
                   };
        }
        #endregion
    }

    public class CategoryCountInfo
    {
        public string CategoryName { get; set; }
        public int CategoryCount {  get; set; }
        public CategoryCountInfo()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is CategoryCountInfo info &&
                   CategoryName == info.CategoryName &&
                   CategoryCount == info.CategoryCount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CategoryName, CategoryCount);
        }
    }
}
