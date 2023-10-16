using BlogApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BlogApp.Repository.Database
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("blogs");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(post => post
                .HasOne(post => post.Blog)
                .WithMany(blog => blog.Posts)
                .HasForeignKey(post => post.BlogId)
                .OnDelete(DeleteBehavior.Cascade));

            modelBuilder.Entity<Comment>(comment => comment
                .HasOne(comment => comment.Post)
                .WithMany(post => post.Comments)
                .HasForeignKey(comment => comment.PostId)
                .OnDelete(DeleteBehavior.Cascade)
            );

            Random rnd = new Random();
            List<Blog> blogs = new List<Blog>();
            for (int i = 1; i <= 50; i++)
            {
                blogs.Add(new Blog(i, $"blog{i}", $"https://boiler_plate{i}.blog"));
            }

            List<Post> posts = new List<Post>();
            List<Comment> comments = new List<Comment>();
            int cIdx = 1;
            for (int i = 1; i <= 1000;i++)
            {
                string author = $"author{rnd.Next(1, 30)}";
                posts.Add(new Post(i, $"[{author} post's title]", author, $"[{author} post's body]", rnd.Next(1, 50)));
                for (int j = 1; j <= rnd.Next(0, 50); j++)
                {
                    string user = $"user{rnd.Next(50, 700)}";
                    comments.Add(new Comment(cIdx++, user, $"[{user} comment's about {author} post's]", i));
                }
            }

            modelBuilder.Entity<Blog>().HasData(blogs);
            modelBuilder.Entity<Post>().HasData(posts);
            modelBuilder.Entity<Comment>().HasData(comments);
        }
    }
}
