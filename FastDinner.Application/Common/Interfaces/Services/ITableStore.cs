namespace FastDinner.Application.Common.Interfaces.Services
{
    public interface ITableStore
    {
        void CreateIfNotExists();
        Task CreateIfNotExistsAsync();
        Task<List<T>> GetAllPartitionsAsync<T>(string partitionKey) where T : class, new();
        Task<T> FindAsync<T>(string partitionKey, Guid rowkey) where T : class, new();
    }
}
