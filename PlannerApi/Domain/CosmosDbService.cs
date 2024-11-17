using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using PlannerApi.Domain.Entities;

namespace PlannerApi.Domain
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;
        private readonly Database _database;


        public CosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
            _database = cosmosClient.GetDatabase(databaseName);

        }

        public async Task InitializeAsync()
        {
            await _database.CreateContainerIfNotExistsAsync(new ContainerProperties
            {
                Id = CosmosDbContainerName.Users,
                PartitionKeyPath = "/userId"
            });

            await _database.CreateContainerIfNotExistsAsync(new ContainerProperties
            {
                Id = CosmosDbContainerName.Patients,
                PartitionKeyPath = "/userId"
            });

            await _database.CreateContainerIfNotExistsAsync(new ContainerProperties
            {
                Id = CosmosDbContainerName.Appointments,
                PartitionKeyPath = "/userId"
            });
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(string queryString)
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<T> GetItemAsync<T>(string id)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(T);
            }
        }

        public async Task AddItemAsync<T>(T item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.GetType().GetProperty("Id").GetValue(item, null).ToString()));
        }

        public async Task UpdateItemAsync<T>(string id, T item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }

        public async Task DeleteItemAsync<T>(string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }
    }
}