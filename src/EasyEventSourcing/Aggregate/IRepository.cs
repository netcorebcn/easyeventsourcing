using System;
using System.Threading.Tasks;

namespace EasyEventSourcing.Aggregate
{
    public interface IRepository
    {
        Task<TAggregate> GetById<TAggregate>(Guid id) where TAggregate : IAggregate, new();

        Task<int> Save(IAggregate aggregate, bool concurrencyCheck = true);
    }
}