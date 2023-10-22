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
            var comments = new List<Comment>();
            var posts = new List<Post>()
            {
                new Post(1, "[author8 post's title]", "author8", "[author8 post's body]", 1),
                new Post(2, "[author12 post's title]", "author12", "[author12 post's body]", 1),
                new Post(3, "[author23 post's title]", "author23", "[author23 post's body]", 1),
                new Post(4, "[author2 post's title]", "author2", "[author2 post's body]", 2),
                new Post(5, "[author18 post's title]", "author18", "[author18 post's body]", 2),
                new Post(6, "[author8 post's title]", "author8", "[author8 post's body]", 3),
                new Post(7, "[author6 post's title]", "author6", "[author6 post's body]", 3),
                new Post(8, "[author29 post's title]", "author29", "[author29 post's body]", 3),
                new Post(9, "[author3 post's title]", "author3", "[author3 post's body]", 3),
                new Post(10, "[author15 post's title]", "author15", "[author15 post's body]", 3),
                new Post(11, "[author7 post's title]", "author7", "[author7 post's body]", 3),
                new Post(12, "[author17 post's title]", "author17", "[author17 post's body]", 4),
                new Post(13, "[author16 post's title]", "author16", "[author16 post's body]", 4),
                new Post(14, "[author27 post's title]", "author27", "[author27 post's body]", 4),
                new Post(20, "[author27 post's title]", "author27", "[author27 post's body]", 4),
                new Post(15, "[author29 post's title]", "author29", "[author29 post's body]", 5),
                new Post(16, "[author27 post's title]", "author27", "[author27 post's body]", 5),
                new Post(17, "[author16 post's title]", "author16", "[author16 post's body]", 5),
                new Post(18, "[author14 post's title]", "author14", "[author14 post's body]", 5),
                new Post(19, "[author29 post's title]", "author29", "[author29 post's body]", 5)
            };

            var blogs = new List<Blog>()
            {
                new Blog(1, "blog1", "https://boiler_plate1.blog"),
                new Blog(2, "blog2", "https://boiler_plate2.blog"),
                new Blog(3, "blog3", "https://boiler_plate3.blog"),
                new Blog(4, "blog4", "https://boiler_plate4.blog"),
                new Blog(5, "blog5", "https://boiler_plate5.blog")
            };

            //Table connections
            foreach (var blog in blogs)
            {
                blog.Posts = posts.Where(p => p.BlogId == blog.Id).ToList();
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
        public void CreationTest()
        {
            BlogDbContext ctx = new BlogDbContext();
            ;
        }
    }
}
