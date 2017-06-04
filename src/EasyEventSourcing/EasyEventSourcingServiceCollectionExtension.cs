using System;
using System.Reflection;
using EasyEventSourcing;
using EasyEventSourcing.Aggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EasyEventSourcingServiceCollectionExtension
    {
        public static IServiceCollection AddEasyEventSourcing<TAggregateRoot>(
            this IServiceCollection services, 
            IConfiguration configuration)
            where TAggregateRoot : IAggregate =>
                services.AddEasyEventSourcing<TAggregateRoot>(EventStoreOptions.Create(
                        configuration["EVENT_STORE"],
                        configuration["EVENT_STORE_MANAGER_HOST"]));
        
        public static IServiceCollection AddEasyEventSourcing<TAggregateRoot>(
            this IServiceCollection services, 
            EventStoreOptions options = null) 
            where TAggregateRoot : IAggregate
        {
            services = services ?? throw new ArgumentNullException(nameof(services));
            options = options ?? EventStoreOptions.Create();

            var connection = EventStoreConnectionFactory.Create(options.ConnectionString);
            var eventDeserializer = new EventDeserializer(typeof(TAggregateRoot).GetTypeInfo().Assembly);
            var projections = new EventStoreProjectionsClient(options);

            services.AddSingleton(connection);
            services.AddSingleton(eventDeserializer);

            services.AddSingleton<IEventStoreProjections>(projections);
            services.AddSingleton<IEventStoreBus>(new EventStoreSubscription(connection, options, eventDeserializer, projections));

            services.AddTransient<IRepository, EventStoreRepository>();
            services.AddTransient<IEventStore, EventStoreRepository>();

            return services;
        }
    }
}