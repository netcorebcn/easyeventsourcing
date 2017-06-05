using System;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;

namespace EasyEventSourcing
{
    public interface IEventStoreProjections
    {
        Task<string> GetStateAsync(string projectionName);

        Task<Guid> GetCurrentId<TAggregate>() where TAggregate : IAggregate, new();

        Task CreateAsync(string projectionName, string query);
    }
}
