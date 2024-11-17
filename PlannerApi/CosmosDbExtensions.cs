using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlannerApi.Domain;

namespace PlannerApi
{
    public static class CosmosDbExtensions
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<CosmosClient>(serviceProvider =>
            {
                string account = configuration["CosmosDbAccountUri"];
                string key = configuration["CosmosDbKey"];
                return new CosmosClient(account, key);
            });

            services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());

            return services;
        }

        private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            string databaseName = configurationSection["CosmosDbDatabaseName"];
            string containerName = configurationSection["CosmosDbContainerName"];
            string account = configurationSection["CosmosDbAccountUri"];
            string key = configurationSection["CosmosDbKey"];
            CosmosClient client = new CosmosClient(account, key);
            CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;
        }
    }
}