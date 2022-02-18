using AzureFunction.Examples.Services;
using AzureFunction.Examples.Services.Interfaces;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(AzureFunction.Examples.Function.Startup))]
namespace AzureFunction.Examples.Function
{
    public class Startup : FunctionsStartup
    {
        private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables()
                   .Build();

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            builder.Services.AddSingleton(s => {
                var connectionString = Configuration["Azure:Cosmos:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException(
                        "Please specify a valid CosmosDBConnection in the local.settings.json file or your Azure Functions Settings.");

                return new CosmosClientBuilder(connectionString)
                    .Build();
            });

            builder.Services.AddSingleton<IThemeParkRepository, ThemeParkRepository>();
            builder.Services.AddSingleton<IRideRepository, RideRepository>();
            builder.Services.AddLogging();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();
            
            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "local.settings.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }
    }
}