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

        public IEnumerable<CategoryRatingAvgInfo> GetAverageRatingOfPostsPerCategory()
        {
            var blogs = repo.ReadAll().ToList();
            List<Post> posts = new List<Post>();
            blogs.ForEach(b => posts.AddRange(b.Posts));
            var CategoryRatingAvgs = new List<CategoryRatingAvgInfo>();
            foreach (var category in posts.GroupBy(p => p.Category))
            {

                List<Comment> comments = new List<Comment>();
                foreach (var post in category)
                {
                    comments.AddRange(post.Comments);
                }
                double avg = comments.Average(c => c.PostRating);
                CategoryRatingAvgs.Add(new CategoryRatingAvgInfo()
                {
                    CategoryName = category.Key,
                    CategoryRatingAvg = avg
                });
            }
            return CategoryRatingAvgs;
        }
        #endregion
    }

    public class CategoryRatingAvgInfo
    {
        public CategoryRatingAvgInfo()
        {
        }

        public string CategoryName { get; set; }
        public double CategoryRatingAvg { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CategoryRatingAvgInfo info &&
                   CategoryName == info.CategoryName &&
                   CategoryRatingAvg == info.CategoryRatingAvg;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CategoryName, CategoryRatingAvg);
        }
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
