using BlogApp.Logic.Classes;
using BlogApp.Models;
using BlogApp.Repository.Database;
using BlogApp.Repository.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogApp.Test
{
    [TestFixture]
    public class BlogAppTester
    {
        Mock<IRepository<Blog>> bRepoMock;
        Mock<IRepository<Post>> pRepoMock;
        Mock<IRepository<Comment>> cRepoMock;
        BlogLogic bl;
        PostLogic pl;
        CommentLogic cl;


        [SetUp]
        public void Init()
        {
            var blogs = new List<Blog>()
            {
                new Blog(1, "blog1", "https://boiler_plate1.blog"),
                new Blog(2, "blog2", "https://boiler_plate2.blog")
            };

            int[] numberOfPostsPerBlog = new int[] { 3, 2};
            int idx = 1;
            var posts = new List<Post>();
            string[] categories = new string[] { "cat1", "cat2"};
            for (int i = 0; i < numberOfPostsPerBlog.Length; i++)
            {
                for (int j = 0; j < numberOfPostsPerBlog[i]; j++)
                {
                    posts.Add(new Post()
                    {
                        Id = idx++,
                        PostTitle = $"[author{idx - 1} post's title]",
                        PostAuthor = $"author{idx - 1}",
                        PostBody = $"[author{idx - 1} post's body]",
                        BlogId = i + 1,
                        Category = categories[idx % categories.Length]
                    }
                    );
                }
            }

            int[] numberOfCommentsPerPost = new int[] { 4, 2, 3 , 2, 1};
            idx = 1;
            var comments = new List<Comment>();
            for (int i = 0; i < numberOfCommentsPerPost.Length; i++)
            {
                for (int j = 0; j < numberOfCommentsPerPost[i]; j++)
                {
                    comments.Add(new Comment() 
                    { 
                        Id = idx++, 
                        UserName = $"user{idx - 1}", 
                        CommentBody = $"[user{idx - 1} comment's about author{i + 1} post's]", 
                        PostId = i + 1, 
                        PostRating = idx % 10 + 1
                    }
                    );
                }
            }

            //Table connections
            foreach (var blog in blogs)
            {
                blog.Posts = posts.Where(p => p.BlogId == blog.Id).ToList();
            }
            foreach (var post in posts)
            {
                post.Comments = comments.Where(c => c.PostId == post.Id).ToList();
            }

            bRepoMock = new Mock<IRepository<Blog>>();
            pRepoMock = new Mock<IRepository<Post>>();
            cRepoMock = new Mock<IRepository<Comment>>();

            bRepoMock.Setup(b => b.ReadAll()).Returns(blogs.AsQueryable());
            pRepoMock.Setup(p => p.ReadAll()).Returns(posts.AsQueryable());
            cRepoMock.Setup(c => c.ReadAll()).Returns(comments.AsQueryable());

            bl = new BlogLogic(bRepoMock.Object);
            pl = new PostLogic(pRepoMock.Object);
            cl = new CommentLogic(cRepoMock.Object);
        }

        [Test]
        public void GetPostsCountPerCategoryTest()
        {
            var expected = new List<CategoryPostCountInfo>
            {
                new CategoryPostCountInfo()
                {
                    CategoryName = "cat1",
                    CategoryCount = 3
                },
                new CategoryPostCountInfo()
                {
                    CategoryName = "cat2",
                    CategoryCount = 2
                }
            };
            var actual = bl.GetPostsCountPerCategory();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAverageRatingOfPostsPerCategoryTest()
        {
            var expected = new List<CategoryAvgPostRatingInfo>
            {
                new CategoryAvgPostRatingInfo()
                {
                    CategoryName = "cat1",
                    CategoryAvgPostRating = 5.25
                },
                new CategoryAvgPostRatingInfo()
                {
                    CategoryName = "cat2",
                    CategoryAvgPostRating = 5
                }
            };
            var actual = bl.GetAverageRatingOfPostsPerCategory();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAverageNumberOfCommentsPerPostTest()
        {
            var expected = new List<AvgNumberOfCommentsInfo>
            {
                new AvgNumberOfCommentsInfo()
                {
                    BlogName = "blog1",
                    AvgNumberOfComments = 3
                },
                new AvgNumberOfCommentsInfo()
                {
                    BlogName = "blog2",
                    AvgNumberOfComments = 1.5
                }
            };
            var actual = bl.GetAverageNumberOfCommentsPerPost();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetMostPopularPostPerBlogTest()
        {
            var expected = new List<MostPopularPostInfo>
            {
                new MostPopularPostInfo()
                {
                    BlogName = "blog1",
                    MostPopularPostTitle = "[author1 post's title]",
                    NumberOfComments = 4
                },
                new MostPopularPostInfo()
                {
                    BlogName = "blog2",
                    MostPopularPostTitle = "[author4 post's title]",
                    NumberOfComments = 2
                }
            };
            var actual = bl.GetMostPopularPostPerBlog();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetBlogRankingsByPopularityTest()
        {
            var expected = new List<BlogRankingInfo>()
            {
                new BlogRankingInfo()
                {
                    BlogName = "blog1",
                    TotalNumberOfComments = 9
                },
                new BlogRankingInfo()
                {
                    BlogName = "blog2",
                    TotalNumberOfComments = 3
                }
            };
            var actual = bl.GetBlogRankingsByPopularity();
            Assert.AreEqual(expected, actual);
        }
    }
}
