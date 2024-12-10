using System.Collections.Concurrent;
using Elsa.Extensions;
using Elsa.Features.Services;
using Microsoft.Extensions.DependencyInjection;
using ZyphCare.EntityFramework.Common.Features;

namespace ZyphCare.EntityFramework.Common.Extensions;

public static class ModuleExtensions
{
    private static readonly IDictionary<IServiceCollection, IModule> Modules =
        new ConcurrentDictionary<IServiceCollection, IModule>();

    public static IModule AddZyphCareEntityFramework(this IServiceCollection serviceCollection,
        Action<IModule>? configure = default)
    {
        var module = serviceCollection.GetOrCreateModule();
        module.Configure<AppFeature>(app => app.Configurator = configure);
        module.Apply();
        return module;
    }

    private static IModule GetOrCreateModule(this IServiceCollection services)
    {
        if (Modules.TryGetValue(services, out var module))
            return module;

        module = services.CreateModule();

        Modules[services] = module;
        return module;
    }
}