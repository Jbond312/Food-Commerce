using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Foods.Service.Api.Middleware
{
    public class RateLimitingMiddleware
    {
        private const int EmptyString = 1;
        private const int NextResource = 2;
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly RateLimiting _config;

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache, IOptions<RateLimiting> options)
        {
            _next = next;
            _cache = cache;
            _config = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = GetApiHit(context);

            var rule = GetRule(request);

            var uniqueKey = request.Ip + request.Route;

            if (_cache.TryGetValue(uniqueKey, out CacheItem cacheItem))
            {
                if (cacheItem.Counter >= rule.Limit)
                {
                    //return error
                    context.Response.StatusCode = 429; //Too many requests error code
                }
                else
                {
                    cacheItem.Counter++;
                    await UpsertCacheEntry(context, uniqueKey, cacheItem);
                }
            }
            else
            {
                var toCache = new CacheItem
                {
                    Counter = 1,
                    Expiration = DateTime.Now.AddSeconds(rule.PeriodInSeconds)
                };
                await UpsertCacheEntry(context, uniqueKey, toCache);
            }
        }

        private async Task UpsertCacheEntry(HttpContext context, 
            string uniqueKey,
            CacheItem cacheItem)
        {
            _cache.Set(uniqueKey, cacheItem, cacheItem.Expiration);

            await _next.Invoke(context);
        }

        private RateLimitRule GetRule(ApiHitRequest request)
        {
            return _config.Rules.FirstOrDefault(x => x.EndPoint == request.Route) 
                ?? _config.Rules.FirstOrDefault(x => x.EndPoint == "*");
        }

        private static ApiHitRequest GetApiHit(HttpContext context)
        {
            var route = context.Request.Path;

            var routeSplit = route.Value.Split("/").Skip(EmptyString).ToList();
            var newRoute = "/api";
            for (var i = 1; i < routeSplit.Count(); i += NextResource)
            {
                var resource = routeSplit[i];

                var replacer = "";
                if (i + 1 < routeSplit.Count)
                {
                    replacer = "/*";
                }
                
                newRoute += "/" + resource + replacer;
            }
            return new ApiHitRequest
            {
                Ip = context.Request.Headers["ip"],
                Route = newRoute
            };
        }
    }

    public class ApiHitRequest
    {
        public string Ip { get; set; }
        public string Route { get; set; }
    }

    public class CacheItem
    {
        public DateTime Expiration { get; set; }
        public int Counter { get; set; }
    }
}