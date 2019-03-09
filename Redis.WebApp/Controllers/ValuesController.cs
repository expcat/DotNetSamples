using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Redis.Extension;
using Redis.WebApp.Models;
using StackExchange.Redis;

namespace Redis.WebApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly IDistributedCache _cache;

        public ValuesController(IConnectionMultiplexer connection, IDistributedCache cache)
        {
            _connection = connection;
            _cache = cache;
        }

        [HttpGet]
        public async Task<Customer> Get(string key = "default")
        {
            Customer customer = await _cache.GetLz4Async<Customer>(key);
            if (customer is null)
            {
                customer = new Customer
                {
                    Name = key,
                    Age = DateTime.Now.Millisecond % 99
                };
                await _cache.SetLz4Async(key, customer, TimeSpan.FromSeconds(20));
            }
            return customer;
        }

        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            return _connection.GetServer(ConfigHelper.host).Keys(pattern: "cache:*").Select(key => key.ToString());
        }
    }
}
