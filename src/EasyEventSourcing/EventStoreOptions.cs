using System;
using EventStore.ClientAPI.SystemData;

namespace EasyEventSourcing
{
    public class EventStoreOptions
    {    
        public string ConnectionString { get; }
        public string ManagerHost { get; }
        public (string stream, string group) Subscription { get; }
        public UserCredentials Credentials { get; } = new UserCredentials("admin", "changeit");

        private EventStoreOptions(string connectionString, string managerHost, (string, string) subscription)
        {
            ConnectionString = connectionString;
            ManagerHost = managerHost;
            Subscription = subscription;
        }
        
        public static EventStoreOptions Create(
            string eventStore = "tcp://admin:changeit@localhost:1113", 
            string eventStoreManagerHost = "localhost:2113", 
            string stream = "Default") =>
                new EventStoreOptions(eventStore, eventStoreManagerHost, (stream,  Guid.NewGuid().ToString()));
    }
}