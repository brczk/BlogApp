using BlogApp.Endpoint.Services;
using BlogApp.Logic.Classes;
using BlogApp.Logic.Interfaces;
using BlogApp.Models;
using BlogApp.Repository.Database;
using BlogApp.Repository.Interfaces;
using BlogApp.Repository.ModelRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace BlogApp.Endpoint
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<BlogDbContext>();

            services.AddTransient<IRepository<Blog>, BlogRepository>();
            services.AddTransient<IRepository<Post>, PostRepository>();
            services.AddTransient<IRepository<Comment>, CommentRepository>();

            services.AddTransient<IBlogLogic, BlogLogic>();
            services.AddTransient<IPostLogic, PostLogic>();
            services.AddTransient<ICommentLogic, CommentLogic>();

            services.AddSignalR();

            services.AddControllers();
            services.AddSwaggerGen(t => t.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "BlogApp.Endpoint",
                Version = "v1",
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(t => t.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogApp.Endpoint v1"));
            }

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { Msg = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalRHub>("/hub");
            });
        }
    }
}
