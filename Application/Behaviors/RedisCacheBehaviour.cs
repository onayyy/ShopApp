using Application.Common.Interfaces.RedisCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Behaviors
{
    public class RedisCacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IRedisCacheService _redisCacheService;

        public RedisCacheBehaviour(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
         
            if (request is ICacheableQuery query)
            {
                var cacheKey = query.CacheKey;
                var cacheTime = query.CacheTime;

                var cachedData = await _redisCacheService.GetAsync<TResponse>(cacheKey);
                if (cachedData is not null)
                    return cachedData;

                var response = await next();
                if (response is not null)
                    await _redisCacheService.SetAsync<TResponse>(cacheKey, response, DateTime.Now.AddMinutes(cacheTime));

                return response;
            }

            return await next();

        }
    }
}
