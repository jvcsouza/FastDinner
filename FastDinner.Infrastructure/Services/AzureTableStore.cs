using Azure;
using Azure.Data.Tables;
using FastDinner.Application.Common.Interfaces.Services;
using FastDinner.Infrastructure.Utils;
using Newtonsoft.Json;
using System.Reflection;
using FastDinner.Domain.Model;

namespace FastDinner.Infrastructure.Services
{
    public class AzureTableStore : ITableStore
    {
        private readonly TableClient _client;

        public AzureTableStore(string connectionStr, string tableName)
        {
#if DEBUG
            _client = new TableClient(
                "DefaultEndpointsProtocol=https;AccountName=fastdinner;AccountKey=1beIqkfV9+1uiqeu952tstRguVF5c0r6DGMTzDHQYgG1kDM002WAfctkeJAr2EIuJZCYsx++8P94+AStUIktlA==;EndpointSuffix=core.windows.net",
                tableName);
#else
            _client = new TableClient(connectionStr, tableName);
#endif
        }

        public void CreateIfNotExists()
        {
            _client.CreateIfNotExists();
        }

        public async Task CreateIfNotExistsAsync()
        {
            await _client.CreateIfNotExistsAsync();
        }

        public async Task<List<T>> GetAllPartitionsAsync<T>(string partitionKey) where T : class, new()
        {
            var entities = _client.QueryAsync<TableEntity>($"PartitionKey eq '{partitionKey}'");

            var lst = new List<T>();

            await foreach (var entity in entities)
            {
                lst.Add(TableEntityMap<T>.Map(entity));
            }

            return lst;
        }

        public async Task<T> FindAsync<T>(string partitionKey, Guid rowKey) where T : class, new()
        {
            try
            {
                var entity = await _client.GetEntityAsync<TableEntity>(partitionKey, rowKey.ToString().ToUpper());

                var response = entity.GetRawResponse();

                return response.Content.ToObjectFromJson<T>();
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return default(T);
            }
        }

        public async Task InsertAsync<T>(string partitionKey, T data) where T : class
        {
            await InsertAsync(partitionKey, Guid.NewGuid(), data);
        }

        public Task InsertAsync<T>(string partitionKey, Guid rowKey, T data) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
