using System;
using EventStore.ClientAPI.SystemData;

namespace EasyEventSourcing
{
    public class EventStoreOptions
    {
        public string ConnectionString { get; }

        public string ManagerHost { get; }

        public string GroupSubscription { get; } = Guid.NewGuid().ToString();

        public UserCredentials Credentials { get; }

        private EventStoreOptions(string connectionString, string managerHost)
        {
            var parsedManagerHost = TryParseManagerHost();

            ConnectionString = connectionString;
            ManagerHost = parsedManagerHost.host;
            Credentials = new UserCredentials(
                parsedManagerHost.credentials.user,
                parsedManagerHost.credentials.password);

            ((string user, string password) credentials, string host) TryParseManagerHost()
            {
                var managerHostParts = managerHost.Split('@');
                return managerHostParts.Length == 2
                    ? (credentials: TryParseCredentials(managerHostParts[0]), host: managerHostParts[1])
                    : throw new ArgumentException(nameof(managerHostParts));

                (string credentials, string host) TryParseCredentials(string credentials)
                {
                    var credentialsParts = credentials.Split(':');
                    return credentialsParts.Length == 2
                        ? (user: credentialsParts[0], password: credentialsParts[1])
                        : throw new ArgumentException(nameof(credentialsParts));
                }
            }
        }

        public static EventStoreOptions Create(string eventStore = null, string eventStoreApi = null) =>
                new EventStoreOptions(
                    eventStore ?? "tcp://admin:changeit@localhost:1113",
                    eventStoreApi ?? "admin:changeit@localhost:2113");
    }
}