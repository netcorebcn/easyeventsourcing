using System;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;
using EventStore.ClientAPI;

namespace EasyEventSourcing
{
    public interface IEventStoreBus
    {
        Task Subscribe(string streamName, Func<Guid, object, Task> messageSender);

        Task Subscribe<TAggregate>(Func<Guid, object, Task> messageSender) where TAggregate : IAggregate;
    }
}