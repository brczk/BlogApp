using BlogApp.Repository.Database;
using NUnit.Framework;
using System;
using System.Linq;

namespace BlogApp.Test
{
    [TestFixture]
    public class BlogRepoTester
    {
        [Test]
        public void CreationTest()
        {
            BlogDbContext ctx = new BlogDbContext();
        }
    }
}
