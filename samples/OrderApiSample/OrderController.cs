using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;
using Microsoft.AspNetCore.Mvc;

namespace OrderApiSample
{
    [Route("api/[controller]")]
    public class OrderController
    {
        private readonly IRepository _repo;

        public OrderController (IRepository repo) => _repo = repo;

        [HttpPost]
        public async Task Create([FromBody]CreateOrderCommand command)
        {
            var aggregate = new OrderAggregate(command.Description);
            await _repo.Save(aggregate);
        }
    }
}
