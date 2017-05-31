using System;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;

namespace EasyEventSourcing
{
    public interface IEventStoreBus
    {
        Task Subscribe(string streamName, Func<object, Task> messageSender);

        Task Subscribe<TAggregate>(Func<object, Task> messageSender) where TAggregate : IAggregate;
    }
}