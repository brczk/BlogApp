using BlogApp.Logic.Interfaces;
using BlogApp.Models;
using BlogApp.Models.Helpers;
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

        public Blog Read(int id)
        {
            var blog = repo.Read(id);
            if (blog == null)
            {
                throw new NullReferenceException("Non-existent blog");
            }
            return blog;
        }

        public IQueryable<Blog> ReadAll()
        {
            return repo.ReadAll();
        }

        public void Update(Blog item)
        {
            this.Read(item.Id);
            if (item.BlogName.Length < 5)
            {
                throw new ArgumentException("Short blog name");
            }
            repo.Update(item);
        }

        #endregion
        #region Non-CRUD
        public IEnumerable<AvgNumberOfCommentsInfo> GetAverageNumberOfCommentsPerPost()
        {
            var blogs = repo.ReadAll().ToList();
            var AverageNumberOfCommentsPerPost = new List<AvgNumberOfCommentsInfo>();
            foreach (var blog in blogs)
            {
                double TotalNumberOfComments = 0;
                foreach (var post in blog.Posts)
                {
                    TotalNumberOfComments += post.Comments.Count;
                }
                AverageNumberOfCommentsPerPost.Add(new AvgNumberOfCommentsInfo()
                {
                    BlogName = blog.BlogName,
                    AvgNumberOfComments = TotalNumberOfComments / (blog.Posts.Count == 0 ? 1 : blog.Posts.Count)
                });
            }
            return AverageNumberOfCommentsPerPost;
        }
        public IEnumerable<BlogRankingInfo> GetBlogRankingsByPopularity()
        {
            var blogs = repo.ReadAll();
            var ranking = new List<BlogRankingInfo>();
            foreach (var blog in blogs)
            {
                int TotalNumberOfComments = 0;
                foreach (var post in blog.Posts)
                {
                    TotalNumberOfComments += post.Comments.Count;
                }
                ranking.Add(new BlogRankingInfo()
                {
                    BlogName = blog.BlogName,
                    TotalNumberOfComments = TotalNumberOfComments
                });
            }
            return ranking.OrderByDescending(x => x.TotalNumberOfComments);
        }

        public IEnumerable<MostPopularPostInfo> GetMostPopularPostPerBlog()
        {
            var blogs = repo.ReadAll().ToList();
            var MostPopularPosts = new List<MostPopularPostInfo>();
            foreach (var blog in blogs)
            {
                var MostPopularPost = blog.Posts.OrderByDescending(p => p.Comments.Count).FirstOrDefault();
                if (MostPopularPost != null)
                {
                    MostPopularPosts.Add(new MostPopularPostInfo()
                    {
                        BlogName = blog.BlogName,
                        MostPopularPostContent = MostPopularPost.Content,
                        NumberOfComments = MostPopularPost.Comments.Count
                    });
                }
            }
            return MostPopularPosts;
        }

        public IEnumerable<CategoryPostCountInfo> GetPostsCountPerCategory()
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
                double avg = 0;
                if(comments.Count > 0)
                {
                    avg = comments.Average(c => c.PostRating);
                }
                CategoryPostRatingAvgs.Add(new CategoryAvgPostRatingInfo()
                {
                    CategoryName = category.Key,
                    CategoryAvgPostRating = avg
                });
            }
            return CategoryPostRatingAvgs;
        }
        #endregion
    }
}
