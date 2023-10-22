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
            var comments = new List<Comment>()
            {

            };


            int[] numberOfPosts = new int[]{ 3, 2, 6, 4, 5 };
            int idx = 1;
            var posts = new List<Post>();
            for (int i = 0; i < numberOfPosts.Length; i++)
            {
                for (int j = 0; j < numberOfPosts[i]; j++)
                {
                    posts.Add(new Post(idx++, $"[author{i + 1} post's title]", $"author{i + 1}", $"[author{i + 1} post's body]", i + 1));
                }
            }
            
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
