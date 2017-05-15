using System;
using System.Reflection;
using EasyEventSourcing;
using EasyEventSourcing.Aggregate;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EasyEventSourcingServiceCollectionExtension
    {
        public static IServiceCollection AddEasyEventSourcing(this IServiceCollection services, EventStoreOptions options, Assembly eventsAssembly)
        {
            services = services ?? throw new ArgumentNullException(nameof(services));
            options = options ?? throw new ArgumentNullException(nameof(options));
            eventsAssembly = eventsAssembly ?? throw new ArgumentNullException(nameof(eventsAssembly));


            var connection = EventStoreConnectionFactory.Create(options.ConnectionString);
            var eventDeserializer = new EventDeserializer(eventsAssembly);

            services.AddSingleton(connection);
            services.AddSingleton(eventDeserializer);

            services.AddSingleton<IEventStoreProjections>(new EventStoreProjectionsClient(options));
            services.AddSingleton<IEventStoreBus>(new EventStoreSubscription(connection, options, eventDeserializer));
            services.AddTransient<IRepository, EventStoreRepository>();

            return services;
        }
    }
}