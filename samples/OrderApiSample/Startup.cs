using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EasyEventSourcing;
using EasyEventSourcing.Aggregate;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace OrderApiSample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer().AddJsonFormatters();
            services.AddSwaggerGen(c => 
                c.SwaggerDoc("v1", new Info { Title = "Voting API", Version = "v1" })
            );

            services.AddEasyEventSourcing(
                EventStoreOptions.Create(
                    Configuration["EVENT_STORE"], 
                    Configuration["EVENT_STORE_MANAGER_HOST"], 
                    Configuration["STREAM_NAME"]), 
                typeof(OrderCreatedEvent).GetTypeInfo().Assembly);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IEventStoreBus eventBus)
        {
            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voting API"));
        }
    }
}
