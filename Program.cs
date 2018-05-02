using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace webjobnetcore
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var loggerFactory = new LoggerFactory()
                .AddConsole(configuration.GetSection("Logging:Console"));

            IServiceCollection services = new ServiceCollection();
            ConfigureServices(configuration, services, loggerFactory);

            var hostConfiguration = new JobHostConfiguration();
            hostConfiguration.LoggerFactory = loggerFactory;
            hostConfiguration.Queues.MaxPollingInterval = TimeSpan.FromSeconds(10);
            hostConfiguration.Queues.BatchSize = 1;
            hostConfiguration.JobActivator = new InjectableJobActivator(services.BuildServiceProvider());
            hostConfiguration.UseTimers();
            hostConfiguration.UseServiceBus(new ServiceBusConfiguration
            {
                ConnectionString = configuration["ServiceBus:ConnectionString"]
            });

            var host = new JobHost(hostConfiguration);
            host.RunAndBlock();
        }

        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services, ILoggerFactory loggerFactory)
        {
            services.AddScoped<Functions>();
            services.AddScoped<IProcessor, NullProcessor>();
            services.AddSingleton(loggerFactory);
            services.AddSingleton(configuration);

            Environment.SetEnvironmentVariable("AzureWebJobsDashboard", configuration["AzureWebJobsDashboard"]);
            Environment.SetEnvironmentVariable("AzureWebJobsStorage", configuration["AzureWebJobsStorage"]);
        }
    }
}
