
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;

namespace EasyEventSourcing
{
    public interface IEventStore 
    {
        Task<IEnumerable<object>> GetEventStream<TAggregate>(Guid id) where TAggregate : IAggregate, new();
    }
}