using BlogApp.Logic.Interfaces;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogApp.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        ICommentLogic logic;

        public CommentController(ICommentLogic logic)
        {
            this.logic = logic;
        }

        [HttpGet]
        public IEnumerable<Comment> ReadAll()
        {
            return this.logic.ReadAll();
        }

        [HttpGet("{id}")]
        public Comment Read(int id)
        {
            return this.logic.Read(id);
        }

        [HttpPost]
        public void Create([FromBody] Comment value)
        {
            this.logic.Create(value);
        }

        [HttpPut]
        public void Update([FromBody] Comment value)
        {
            this.logic.Update(value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.logic.Delete(id);
        }
    }
}
