using System;


using Microsoft.Extensions.Caching.Memory;
using WebApi.DTOs;
namespace WebApi.CachService;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetTopProductsAsync(CancellationToken ct);
    void InvalidateTopProducts();
}

public class ProductService : IProductService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<ProductService> _logger;

    // Имя ключа кеша. Делай ключи предсказуемыми.
    private const string TopProductsCacheKey = "products:top";

    public ProductService(IMemoryCache cache, ILogger<ProductService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProductDto>> GetTopProductsAsync(CancellationToken ct)
    {
        // GetOrCreateAsync: если ключ есть — вернёт из кеша; иначе создаст
        return await _cache.GetOrCreateAsync(TopProductsCacheKey, async entry =>
        {
            // Настройки жизни кеша
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5); // абсолютная
            entry.SlidingExpiration = TimeSpan.FromMinutes(1); // sliding (см. ниже)
            entry.Priority = CacheItemPriority.High;

            _logger.LogInformation("Cache MISS -> loading top products...");

            // Тут представь что это запрос в БД:
            await Task.Delay(1200, ct);

            return new List<ProductDto>
            {
                new(Guid.NewGuid(), "Keyboard", 35),
                new(Guid.NewGuid(), "Mouse", 15),
            };
        }) ?? new List<ProductDto>();
    }

    public void InvalidateTopProducts()
    {
        _cache.Remove(TopProductsCacheKey);
        _logger.LogInformation("Cache invalidated: {Key}", TopProductsCacheKey);
    }
}