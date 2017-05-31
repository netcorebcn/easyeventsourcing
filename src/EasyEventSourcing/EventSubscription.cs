using System;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;
using EventStore.ClientAPI;

namespace EasyEventSourcing
{
    internal class EventStoreSubscription : IEventStoreBus
    {
        private readonly IEventStoreConnection _conn;
        private readonly EventStoreOptions _options;
        private readonly EventDeserializer _eventDeserializer;
        private readonly IEventStoreProjections _projections;

        public EventStoreSubscription(IEventStoreConnection conn, EventStoreOptions options, EventDeserializer eventDeserializer, IEventStoreProjections projections)
        {
            _conn = conn;
            _options = options;
            _eventDeserializer = eventDeserializer;
            _projections = projections;
        }

        public async Task Subscribe<TAggregate>(Func<object, Task> messageSender) 
            where TAggregate : IAggregate
        {
            var streamName = $"{typeof(TAggregate).Name}Stream";
            await _projections.CreateAsync(streamName, DefaultProjection.Default(typeof(TAggregate).Name, streamName));
            await Subscribe(streamName, messageSender);
        }

        public async Task Subscribe(string streamName, Func<object, Task> messageSender)
        {
            await CreateSubscription();

            await _conn.ConnectToPersistentSubscriptionAsync(
                streamName, _options.GroupSubscription,
                (_, x) => messageSender(_eventDeserializer.Deserialize(x.Event)));


            async Task CreateSubscription()
            {
                var settings = PersistentSubscriptionSettings.Create()
                    .ResolveLinkTos()
                    .StartFromCurrent();

                await _conn.CreatePersistentSubscriptionAsync(streamName, _options.GroupSubscription, settings, _options.Credentials);
            }
        }
    }
}