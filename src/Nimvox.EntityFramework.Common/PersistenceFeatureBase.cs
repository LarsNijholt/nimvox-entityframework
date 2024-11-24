using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nimvox.EntityFramework.Common.Entities;

namespace Nimvox.EntityFramework.Common;

/// <summary>
///     Base class for features that require Entity Framework Core.
/// </summary>
/// <typeparam name="TDbContext">The type of the database context.</typeparam>
public abstract class PersistenceFeatureBase<TDbContext> : FeatureBase where TDbContext : DbContext
{
    public Action<IServiceProvider, DbContextOptionsBuilder> DbContextOptionsBuilder = (_, options) => options
        .UseNimvoxDbContextOptions(default)
        .UseSqlite("Data Source=elsa.sqlite.db;Cache=Shared;", sqlite => sqlite
            .MigrationsAssembly("Nimvox.EntityFrameworkCore.Sqlite")
            .MigrationsHistoryTable(NimvoxDbContextBase.MigrationsHistoryTable, NimvoxDbContextBase.NimvoxSchema));

    /// <inheritdoc />
    protected PersistenceFeatureBase(IModule module) : base(module)
    {
    }

    /// <summary>
    ///     Gets or sets a value indicating whether to use context pooling.
    /// </summary>
    public bool UseContextPooling { get; set; } = false;

    /// <summary>
    ///     Gets or sets a value indicating whether to run migrations.
    /// </summary>
    public bool RunMigrations { get; set; } = true;

    /// <summary>
    ///     Gets or sets the lifetime of the <see cref="IDbContextFactory{TContext}" />. Defaults to
    ///     <see cref="ServiceLifetime.Singleton" />.
    /// </summary>
    public ServiceLifetime DbContextFactoryLifetime { get; set; } = ServiceLifetime.Scoped;

    /// <inheritdoc />
    public override void ConfigureHostedServices()
    {
        if (RunMigrations)
            Module.ConfigureHostedService<RunMigrationsHostedService<TDbContext>>(
                -100); // Migrations need to run before other hosted services that depend on DB access.
    }

    /// <inheritdoc />
    public override void Apply()
    {
        if (UseContextPooling)
            Services.AddPooledDbContextFactory<TDbContext>(DbContextOptionsBuilder);
        else
            Services.AddDbContextFactory<TDbContext>(DbContextOptionsBuilder, DbContextFactoryLifetime);
    }

    /// <summary>
    /// Add an entity store to the service collection.
    /// </summary>
    /// <typeparam name="TEntity">The entity the store is for.</typeparam>
    /// <typeparam name="TStore">the entity store.</typeparam>
    protected void AddEntityStore<TEntity, TStore>() where TEntity : Entity, new() where TStore : class
    {
        Services
            .AddScoped<EntityStore<TDbContext, TEntity>>()
            .AddScoped<TStore>();
    }
}