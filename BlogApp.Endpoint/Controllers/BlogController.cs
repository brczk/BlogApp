using BlogApp.Endpoint.Services;
using BlogApp.Logic.Interfaces;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogApp.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        IBlogLogic logic;
        IHubContext<SignalRHub> hub;

        public BlogController(IBlogLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }

        [HttpGet]
        public IEnumerable<Blog> ReadAll()
        {
            return this.logic.ReadAll();
        }

        [HttpGet("{id}")]
        public Blog Read(int id)
        {
            return this.logic.Read(id);
        }

        [HttpPost]
        public void Create([FromBody] Blog value)
        {
            this.logic.Create(value);
            this.hub.Clients.All.SendAsync("BlogCreated", value);
        }

        [HttpPut]
        public void Update([FromBody] Blog value)
        {
            this.logic.Update(value);
            this.hub.Clients.All.SendAsync("BlogUpdated", value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var blogToDelete = this.logic.Read(id);
            this.logic.Delete(id);
            this.hub.Clients.All.SendAsync("BlogDeleted", blogToDelete);
        }
    }
}
