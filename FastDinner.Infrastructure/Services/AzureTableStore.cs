using Azure;
using Azure.Data.Tables;
using FastDinner.Application.Common.Interfaces.Services;
using FastDinner.Infrastructure.Utils;
using Newtonsoft.Json;
using System.Reflection;

namespace FastDinner.Infrastructure.Services
{
    public class AzureTableStore : ITableStore
    {
        private readonly TableClient _client;
        private readonly MethodInfo _methodQuery;
        private readonly MethodInfo _methodFind;

        public AzureTableStore(string connectionStr, string tableName)
        {
            _client = new TableClient(connectionStr, tableName);

            _methodQuery = typeof(TableClient).GetMethod("Query",
                new[] { typeof(string), typeof(int?), typeof(IEnumerable<string>), typeof(CancellationToken) });

            _methodFind = typeof(TableClient).GetMethod("GetEntity",
                new[] { typeof(string), typeof(string), typeof(IEnumerable<string>), typeof(CancellationToken) });

            //_method = typeof(TableClient).GetMethod("QueryAsync",
            //    new Type[] { typeof(string), typeof(int?), typeof(IEnumerable<string>), typeof(CancellationToken) });
        }

        public async Task CreateIfNotExistsAsync()
        {
            await _client.CreateIfNotExistsAsync();
        }

        public Task<List<T>> GetAllPartitionsAsync<T>(string partitionKey) where T : class
        {
            var request = (dynamic)_methodQuery
                .MakeGenericMethod(TypeBuilder.CopyWithParent<T, StoreItem>())
                .Invoke(_client, new object[] { $"PartitionKey eq '{partitionKey}'", null, null, default(CancellationToken) });

            var lst = new List<T>();

            if (request == null) return Task.FromResult(lst);
            
            foreach (var i in request)
            {
                lst.Add(JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject((object)i)));
            }

            return Task.FromResult(lst);
        }

        public async Task<T> FindAsync<T>(string partitionKey, Guid rowKey) where T : class
        {
            //var properties = typeof(T).GetProperties();

            //Console.WriteLine(_methodFind is null);

            //var request = (dynamic)_methodFind
            //    .MakeGenericMethod(new Type[] { MyTypeBuilder.CompileResultType(properties) })
            //    .Invoke(_client, new object[] { partitionKey, rowKey.ToString(), null, default(CancellationToken) });

            //var rs = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(request));

            //return Task.FromResult(rs);

            var list = await GetAllPartitionsAsync<dynamic>(partitionKey);

            var rs = list.FirstOrDefault(x => x.RowKey == rowKey.ToString().ToUpper());

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(rs));
        }

        public async Task InsertAsync<T>(string partitionKey, T data) where T : class
        {
            await InsertAsync(partitionKey, Guid.NewGuid(), data);
        }

        public Task InsertAsync<T>(string partitionKey, Guid rowKey, T data) where T : class
        {
            return Task.FromResult(true);
        }
    }

    public class StoreItem : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
