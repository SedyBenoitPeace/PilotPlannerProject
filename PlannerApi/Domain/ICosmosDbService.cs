using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApi.Domain
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<T>> GetItemsAsync<T>(string query);
        Task<T> GetItemAsync<T>(string id);
        Task AddItemAsync<T>(T item);
        Task UpdateItemAsync<T>(string id, T item);
        Task DeleteItemAsync<T>(string id);
    }
}