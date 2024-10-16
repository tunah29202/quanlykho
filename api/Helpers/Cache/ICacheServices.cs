using Common;
using Services.Core.Contracts;
namespace Helpers.Cache
{
    public interface ICacheServices
    {
        List<string> GetKeys();
        T Get<T>(string key, Func<T> fallback);
        Task<T> GetAsync<T>(string key, Func<Task<T>> fallback);
        void Clear(string key);
        Task ClearAsync(string key);
        Task ClearAllAsync();
    }
}