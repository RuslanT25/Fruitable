using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.CrossCuttingConcerns.Caching.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.DependecyResolve
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<MemoryCacheManager>();

            // Redis cache yapılandırması
            services.AddRedisCache(configuration);

            // Cache manager implementasyonu
            services.AddSingleton<ICacheManager, RedisCacheManager>();
        }

        public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            // RedisOptions yapılandırmasını doğrudan IConfiguration nesnesinden alıyoruz.
            services.Configure<RedisOptions>(configuration.GetSection(nameof(RedisOptions)));

            // RedisOptions nesnesini doğrudan alıyoruz
            var redisOptions = configuration.GetSection(nameof(RedisOptions)).Get<RedisOptions>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisOptions.ConnectionString;
                options.InstanceName = redisOptions.InstanceName;
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisOptions.ConnectionString));
            services.AddSingleton<RedisCacheManager>();
        }
    }
}