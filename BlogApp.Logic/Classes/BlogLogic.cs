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
        public IEnumerable<CategoryPostCountInfo> PostsCountPerCategory()
        {
            var blogs = repo.ReadAll().ToList();
            List<Post> posts = new List<Post>();
            blogs.ForEach(b => posts.AddRange(b.Posts));
 
            return from post in posts
                   group post by post.Category into categories
                   select new CategoryPostCountInfo()
                   {
                       CategoryName = categories.Key,
                       CategoryCount = categories.Count()
                   };
        }

        public IEnumerable<CategoryAvgPostRatingInfo> GetAverageRatingOfPostsPerCategory()
        {
            var blogs = repo.ReadAll().ToList();
            List<Post> posts = new List<Post>();
            blogs.ForEach(b => posts.AddRange(b.Posts));
            var CategoryPostRatingAvgs = new List<CategoryAvgPostRatingInfo>();
            foreach (var category in posts.GroupBy(p => p.Category))
            {
                List<Comment> comments = new List<Comment>();
                foreach (var post in category)
                {
                    comments.AddRange(post.Comments);
                }
                double avg = comments.Average(c => c.PostRating);
                CategoryPostRatingAvgs.Add(new CategoryAvgPostRatingInfo()
                {
                    CategoryName = category.Key,
                    CategoryAvgPostRating = avg
                });
            }
            return CategoryPostRatingAvgs;
        }
        
        public IEnumerable<AvgNumberOfCommentsInfo> GetAverageNumberOfCommentsPerPost()
        {
            var blogs = repo.ReadAll().ToList();
            var AverageNumberOfCommentsPerPost = new List<AvgNumberOfCommentsInfo>();
            foreach (var blog in blogs)
            {
                if (blog.Posts.Count == 0)
                {
                    AverageNumberOfCommentsPerPost.Add(new AvgNumberOfCommentsInfo()
                    {
                        BlogName = blog.BlogName,
                        AvgNumberOfComments = 0
                    });
                }
                else
                {
                    double TotalNumberOfComments = 0;
                    foreach (var post in blog.Posts)
                    {
                        TotalNumberOfComments += post.Comments.Count;
                    }
                    AverageNumberOfCommentsPerPost.Add(new AvgNumberOfCommentsInfo()
                    {
                        BlogName = blog.BlogName,
                        AvgNumberOfComments = TotalNumberOfComments / blog.Posts.Count
                    });
                }
            }
            return AverageNumberOfCommentsPerPost;
        }
        #endregion
    }

    public class AvgNumberOfCommentsInfo
    {
        public string BlogName;
        public double AvgNumberOfComments;
        public AvgNumberOfCommentsInfo()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is AvgNumberOfCommentsInfo info &&
                   BlogName == info.BlogName &&
                   AvgNumberOfComments == info.AvgNumberOfComments;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BlogName, AvgNumberOfComments);
        }
    }

    public class CategoryAvgPostRatingInfo
    {
        public CategoryAvgPostRatingInfo()
        {
        }

        public string CategoryName { get; set; }
        public double CategoryAvgPostRating { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CategoryAvgPostRatingInfo info &&
                   CategoryName == info.CategoryName &&
                   CategoryAvgPostRating == info.CategoryAvgPostRating;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CategoryName, CategoryAvgPostRating);
        }
    }

    public class CategoryPostCountInfo
    {
        public string CategoryName { get; set; }
        public int CategoryCount {  get; set; }
        public CategoryPostCountInfo()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is CategoryPostCountInfo info &&
                   CategoryName == info.CategoryName &&
                   CategoryCount == info.CategoryCount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CategoryName, CategoryCount);
        }
    }


}
