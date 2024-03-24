using BlogApp.Endpoint.Services;
using BlogApp.Logic.Interfaces;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogApp.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        IPostLogic logic;
        IHubContext<SignalRHub> hub;

        public PostController(IPostLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }

        [HttpGet]
        public IEnumerable<Post> ReadAll()
        {
            return this.logic.ReadAll();
        }

        [HttpGet("{id}")]
        public Post Read(int id)
        {
            return this.logic.Read(id);
        }

        [HttpPost]
        public void Create([FromBody] Post value)
        {
            this.logic.Create(value);
            this.hub.Clients.All.SendAsync("PostCreated", value);
        }

        [HttpPut]
        public void Update([FromBody] Post value)
        {
            this.logic.Update(value);
            this.hub.Clients.All.SendAsync("PostUpdated", value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var postToDelete = this.logic.Read(id);
            this.logic.Delete(id);
            this.hub.Clients.All.SendAsync("PostDeleted", postToDelete);
        }
    }
}
