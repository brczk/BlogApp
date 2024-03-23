using BlogApp.Logic.Classes;
using BlogApp.Logic.Interfaces;
using BlogApp.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogApp.Endpoint.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        IBlogLogic logic;

        public StatsController(IBlogLogic logic)
        {
            this.logic = logic;
        }

        [HttpGet]
        public IEnumerable<AvgNumberOfCommentsInfo> GetAvgNumberOfComments()
        {
            return this.logic.GetAverageNumberOfCommentsPerPost();
        }

        [HttpGet]
        public IEnumerable<BlogRankingInfo> GetBlogRankingsByPopularity()
        {
            return this.logic.GetBlogRankingsByPopularity();
        }

        [HttpGet]
        public IEnumerable<MostPopularPostInfo> GetMostPopularPostPerBlog()
        {
            return this.logic.GetMostPopularPostPerBlog();
        }

        [HttpGet]
        public IEnumerable<CategoryPostCountInfo> GetPostsCountPerCategory()
        {
            return this.logic.GetPostsCountPerCategory();
        }

        [HttpGet]
        public IEnumerable<CategoryAvgPostRatingInfo> GetAverageRatingOfPostsPerCategory()
        {
            return this.logic.GetAverageRatingOfPostsPerCategory();
        }
    }
}
