using BlogApp.Logic.Classes;
using BlogApp.Models;
using BlogApp.Models.Helpers;
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
            #region Fake DbSet
            var blogs = new List<Blog>()
            {
                new Blog(1, "blog1"),
                new Blog(2, "blog2")
            };

            var posts = new List<Post>()
            {
                new Post(){ Id = 1, PostAuthor = "author1", Content = "[author1 post's]", BlogId = 1, Category = "cat1"},
                new Post(){ Id = 2, PostAuthor = "author2", Content = "[author2 post's]", BlogId = 1, Category = "cat2"},
                new Post(){ Id = 3, PostAuthor = "author3", Content = "[author3 post's]", BlogId = 1, Category = "cat1"},
                new Post(){ Id = 4, PostAuthor = "author4", Content = "[author4 post's]", BlogId = 2, Category = "cat2"},
                new Post(){ Id = 5, PostAuthor = "author5", Content = "[author5 post's]", BlogId = 2, Category = "cat1"}
            };

            var comments = new List<Comment>()
            {
                new Comment(){ Id = 1, UserName = "user1", Content = "[user1 comment's about author1 post's]", PostId = 1, PostRating = 3},
                new Comment(){ Id = 2, UserName = "user2", Content = "[user2 comment's about author1 post's]", PostId = 1, PostRating = 4},
                new Comment(){ Id = 3, UserName = "user3", Content = "[user3 comment's about author1 post's]", PostId = 1, PostRating = 5},
                new Comment(){ Id = 4, UserName = "user4", Content = "[user4 comment's about author1 post's]", PostId = 1, PostRating = 6},
                new Comment(){ Id = 5, UserName = "user5", Content = "[user5 comment's about author2 post's]", PostId = 2, PostRating = 7},
                new Comment(){ Id = 6, UserName = "user6", Content = "[user6 comment's about author2 post's]", PostId = 2, PostRating = 8},
                new Comment(){ Id = 7, UserName = "user7", Content = "[user7 comment's about author3 post's]", PostId = 3, PostRating = 9},
                new Comment(){ Id = 8, UserName = "user8", Content = "[user8 comment's about author3 post's]", PostId = 3, PostRating = 10},
                new Comment(){ Id = 9, UserName = "user9", Content = "[user9 comment's about author3 post's]", PostId = 3, PostRating = 1},
                new Comment(){ Id = 10, UserName = "user10", Content = "[user10 comment's about author4 post's]", PostId = 4, PostRating = 2},
                new Comment(){ Id = 11, UserName = "user11", Content = "[user11 comment's about author4 post's]", PostId = 4, PostRating = 3},
                new Comment(){ Id = 12, UserName = "user12", Content = "[user12 comment's about author5 post's]", PostId = 5, PostRating = 4}
            };
            #endregion

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

        #region Crud
        #region create
        [Test]
        [Category("Crud")]
        public void CreateValidBlog()
        {
            Blog blog = new Blog()
            { 
                Id = 3,
                BlogName = "blog3",
            };
            bl.Create(blog);
            bRepoMock.Verify(t => t.Create(blog), Times.Once);
        }
        [Test]
        [Category("Crud")]
        public void CreateInvalidBlog()
        {
            Blog blog = new Blog()
            {
                Id = -1,
                BlogName = "bl",
            };
            try
            {
                bl.Create(blog);
            }
            catch
            {

            }
            bRepoMock.Verify(t => t.Create(blog), Times.Never);
        }
        [Test]
        [Category("Crud")]
        public void CreateValidPost()
        {
            Post post = new Post()
            {
                Id = 100,
                Content = "AAAAAAAAAAAAAAAAAAA"
            };
            pl.Create(post);
            pRepoMock.Verify(t => t.Create(post), Times.Once);
        }
        [Test]
        [Category("Crud")]
        public void CreateInvalidPost()
        {
            Post post = new Post()
            {
                Id = -1,
                Content = "kek",
            };
            try
            {
                pl.Create(post);
            }
            catch
            {

            }
            pRepoMock.Verify(t => t.Create(post), Times.Never);
        }
        [Test]
        [Category("Crud")]
        public void CreateValidComment()
        {
            Comment comment = new Comment()
            {
                Id = 100,
                Content = "AAAAAAAAAAAAAAAAAAA"
            };
            cl.Create(comment);
            cRepoMock.Verify(t => t.Create(comment), Times.Once);
        }
        [Test]
        [Category("Crud")]
        public void CreateInvalidComment()
        {
            Comment comment = new Comment()
            {
                Id = -1,
                Content = "kek",
            };
            try
            {
                cl.Create(comment);
            }
            catch
            {

            }
            cRepoMock.Verify(t => t.Create(comment), Times.Never);
        }
        #endregion
        #endregion

        #region Non-crud
        [Test]
        [Category("Non-crud")]
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
        [Category("Non-crud")]
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
        [Category("Non-crud")]
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
        [Category("Non-crud")]
        public void GetMostPopularPostPerBlogTest()
        {
            var expected = new List<MostPopularPostInfo>
            {
                new MostPopularPostInfo()
                {
                    BlogName = "blog1",
                    MostPopularPostContent = "[author1 post's]",
                    NumberOfComments = 4
                },
                new MostPopularPostInfo()
                {
                    BlogName = "blog2",
                    MostPopularPostContent = "[author4 post's]",
                    NumberOfComments = 2
                }
            };
            var actual = bl.GetMostPopularPostPerBlog();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Non-crud")]
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
        #endregion
    }
}
