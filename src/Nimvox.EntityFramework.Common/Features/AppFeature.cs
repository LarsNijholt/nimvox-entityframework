using Elsa.Features.Abstractions;
using Elsa.Features.Services;

namespace Nimvox.EntityFramework.Common.Features;

public class AppFeature : FeatureBase
{
    public AppFeature(IModule module) : base(module)
    {
    }

    /// <summary>
    ///     The configurator to invoke.
    /// </summary>
    public Action<IModule>? Configurator { get; set; }

    /// <inheritdoc />
    public override void Configure()
    {
        Configurator?.Invoke(Module);
    }
}