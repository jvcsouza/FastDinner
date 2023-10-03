namespace FastDinner.Application.Common.Interfaces.Services
{
    public interface ITableStore
    {
        Task CreateIfNotExistsAsync();
        Task<List<T>> GetAllPartitionsAsync<T>(string partitionKey) where T : class;
        Task<T> FindAsync<T>(string partitionKey, Guid rowkey) where T : class;
    }
}
