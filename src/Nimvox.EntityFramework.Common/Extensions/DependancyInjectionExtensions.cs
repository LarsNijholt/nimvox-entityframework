﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nimvox.EntityFramework.Common.Services;

namespace Nimvox.EntityFramework.Common.Extensions;

public static class DependancyInjectionExtensions
{
    public static IServiceCollection AddMemoryStore<TEntity, TStore>(this IServiceCollection services)
        where TStore : class
    {
        services.TryAddSingleton<MemoryStore<TEntity>>();
        services.TryAddScoped<TStore>();
        return services;
    }
}