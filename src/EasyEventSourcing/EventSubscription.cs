using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EasyEventSourcing
{
    public interface IEventStoreBus
    {
        Task Subscribe(Func<object,Task> messageSender);
    }

    internal class EventStoreSubscription : IEventStoreBus
    {
        private readonly IEventStoreConnection _conn;
        private readonly EventStoreOptions _options;
        private readonly EventDeserializer _eventDeserializer;

        public EventStoreSubscription(IEventStoreConnection conn, EventStoreOptions options, EventDeserializer eventDeserializer)
        {
            _conn = conn;
            _options = options;
            _eventDeserializer = eventDeserializer;
        }

        public async Task Subscribe(Func<object,Task> messageSender)
        {
            await CreateSubscription(_conn, _options);
            await _conn.ConnectToPersistentSubscriptionAsync(
                _options.Subscription.stream, _options.Subscription.group, 
                (_, x) => messageSender(_eventDeserializer.Deserialize(x.Event)));
        }

        private static async Task CreateSubscription(IEventStoreConnection conn, EventStoreOptions options)
        {
            var settings = PersistentSubscriptionSettings.Create()
                .ResolveLinkTos()
                .StartFromCurrent();

            await conn.CreatePersistentSubscriptionAsync(options.Subscription.stream, options.Subscription.group, settings, options.Credentials);
        }
    }
}