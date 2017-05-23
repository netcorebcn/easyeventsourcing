﻿using System;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Log;
using EventStore.ClientAPI.Exceptions;
using EventStore.ClientAPI.Projections;

namespace EasyEventSourcing
{
    public interface IEventStoreProjections
    {
        Task<string> GetStateAsync(string projectionName);

        Task<string> GetStateAsync();

        Task CreateAsync(string projectionName);
    }

    public class EventStoreProjectionsClient : IEventStoreProjections
    {
        private readonly EventStoreOptions _options;

        private readonly ILogger _logger;

        public EventStoreProjectionsClient(EventStoreOptions options)
        {
            _options = options;
            _logger = new ConsoleLogger();
        }

        public async Task<string> GetStateAsync(string projectionName)
        {
            var projectionsClient = await CreateProjectionsClient();
            return await projectionsClient.GetStateAsync(projectionName, _options.Credentials);
        }

        public async Task<string> GetStateAsync() => await GetStateAsync(_options.Subscription.stream);

        public async Task CreateAsync(string query)
        {
            var projectionsClient = await CreateProjectionsClient();
            await projectionsClient.EnableAsync("$by_category", _options.Credentials);

            if (await ProjectionExists())
                await projectionsClient.UpdateQueryAsync(_options.Subscription.stream, query, _options.Credentials);
            else
                await projectionsClient.CreateContinuousAsync(_options.Subscription.stream, query, _options.Credentials);

            async Task<bool> ProjectionExists()
            {
                try
                {
                    var projection = await projectionsClient.GetQueryAsync(_options.Subscription.stream, _options.Credentials);
                    return true;
                }
                catch (ProjectionCommandFailedException ex)
                {
                    _logger.Error(ex, "");
                    return false;
                }
            }
        }

        private async Task<ProjectionsManager> CreateProjectionsClient()
        {
            var address = await GetIPEndPointFromHostName(_options.ManagerHost);
            return new ProjectionsManager(_logger, address, TimeSpan.FromSeconds(90));
        }

        private async Task<IPEndPoint> GetIPEndPointFromHostName(string hostName)
        {
            var hostParts = hostName.Split(':');

            if (hostParts[0] == "localhost")
                return new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse(hostParts[1]));

            var addresses = await System.Net.Dns.GetHostAddressesAsync(hostParts[0]);
            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName"
                );
            }
            return new IPEndPoint(addresses[0], int.Parse(hostParts[1])); // Port gets validated here.
        }
    }
}
