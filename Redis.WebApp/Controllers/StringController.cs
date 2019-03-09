using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Redis.Extension;
using Redis.WebApp.Models;
using StackExchange.Redis;

namespace Redis.WebApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StringController : ControllerBase
    {
        private readonly IConnectionMultiplexer _connection;

        public StringController(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        [HttpGet]
        public Customer StringGet(string key = "default")
        {
            var db = _connection.GetDatabase();
            return db.Get<Customer>(key);
        }
    }
}