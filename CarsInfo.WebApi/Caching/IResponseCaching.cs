using System;
using System.Threading.Tasks;

namespace CarsInfo.WebApi.Caching
{
    public interface IResponseCaching
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);

        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
