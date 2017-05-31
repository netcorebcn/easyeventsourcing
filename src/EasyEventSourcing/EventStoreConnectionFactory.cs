using System;
using EventStore.ClientAPI;

namespace EasyEventSourcing
{
    internal static class EventStoreConnectionFactory
    {
        internal static IEventStoreConnection Create(string connectionString)
        {
            connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            
            var settings = ConnectionSettings.Create().KeepReconnecting().KeepRetrying();
            var connection = EventStoreConnection.Create(settings, new System.Uri(connectionString));
            connection.ConnectAsync().Wait();
            return connection;
        }
    }
}