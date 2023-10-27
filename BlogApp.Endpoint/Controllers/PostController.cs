﻿using BlogApp.Logic.Interfaces;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogApp.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        IPostLogic logic;

        public PostController(IPostLogic logic)
        {
            this.logic = logic;
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
        }

        [HttpPut]
        public void Update([FromBody] Post value)
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
