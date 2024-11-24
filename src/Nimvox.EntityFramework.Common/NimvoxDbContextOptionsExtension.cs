using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Nimvox.EntityFramework.Common;

/// <summary>
/// Provides options for configuring Nimvox's Entity Framework Core integration.
/// </summary>
public class NimvoxDbContextOptionsExtension : IDbContextOptionsExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NimvoxDbContextOptionsExtension"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public NimvoxDbContextOptionsExtension(NimvoxDbContextOptions? options)
    {
        Options = options;
    }
    
    /// <summary>
    /// Gets the options.
    /// </summary>
    public NimvoxDbContextOptions? Options { get; }

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