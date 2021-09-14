using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarsInfo.WebApi.Caching
{
    public class ResponseCaching : IResponseCaching
    {
        private readonly IDistributedCache _distributedCache;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public ResponseCaching(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response is null)
            {
                return;
            }

            var jsonResponse = JsonConvert.SerializeObject(response, _jsonSerializerSettings);
            await _distributedCache.SetStringAsync(cacheKey, jsonResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrWhiteSpace(cachedResponse) ? null : cachedResponse;
        }
    }
}
