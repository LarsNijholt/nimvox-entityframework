using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ZyphCare.EntityFramework.Common;

/// <summary>
///     Provides options for configuring ZyphCare's Entity Framework Core integration.
/// </summary>
public class ZyphCareDbContextOptionsExtension : IDbContextOptionsExtension
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ZyphCareDbContextOptionsExtension" /> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public ZyphCareDbContextOptionsExtension(ZyphCareDbContextOptions? options)
    {
        Options = options;
    }

    /// <summary>
    ///     Gets the options.
    /// </summary>
    public ZyphCareDbContextOptions? Options { get; }

    /// <inheritdoc />
    public DbContextOptionsExtensionInfo Info => new CustomDbContextOptionsExtensionInfo(this);

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services)
    {
    }

    /// <inheritdoc />
    public void Validate(IDbContextOptions options)
    {
    }
}