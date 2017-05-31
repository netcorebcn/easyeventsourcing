using System;
using EventStore.ClientAPI.SystemData;

namespace EasyEventSourcing
{
    public class EventStoreOptions
    {    
        public string ConnectionString { get; }
        public string ManagerHost { get; }

        public string GroupSubscription { get; } = Guid.NewGuid().ToString();

        public UserCredentials Credentials { get; } = new UserCredentials("admin", "changeit");

        private EventStoreOptions(string connectionString, string managerHost)
        {
            ConnectionString = connectionString;
            ManagerHost = managerHost;
        }
        
        public static EventStoreOptions Create(
            string eventStore = "tcp://admin:changeit@localhost:1113", 
            string eventStoreManagerHost = "localhost:2113") =>
                new EventStoreOptions(eventStore, eventStoreManagerHost);
    }
}