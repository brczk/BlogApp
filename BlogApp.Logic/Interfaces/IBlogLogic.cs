using BlogApp.Logic.Classes;
using BlogApp.Models;
using BlogApp.Models.Helpers;
using System;
using System.Collections.Generic;
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
        public IEnumerable<BlogRankingInfo> GetBlogRankingsByPopularity();
        public IEnumerable<MostPopularPostInfo> GetMostPopularPostPerBlog();
        public IEnumerable<CategoryPostCountInfo> GetPostsCountPerCategory();
        public IEnumerable<CategoryAvgPostRatingInfo> GetAverageRatingOfPostsPerCategory();
        public IEnumerable<AvgNumberOfCommentsInfo> GetAverageNumberOfCommentsPerPost();
    }
}
