using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nimvox.EntityFramework.Common.Contracts;

namespace Nimvox.EntityFramework.Common;

public abstract class NimvoxDbContextBase : DbContext, INimvoxDbContextSchema
{
    private static readonly ISet<EntityState> ModifiedEntityStates = new HashSet<EntityState>
    {
        EntityState.Added,
        EntityState.Modified
    };

    /// <summary>
    ///     Initializes a new instance of the <see cref="NimvoxDbContextBase" /> class.
    /// </summary>
    protected NimvoxDbContextBase(DbContextOptions options) : base(options)
    {
        var nimvoxDbContextOptions = options.FindExtension<NimvoxDbContextOptionsExtension>()?.Options;

        Schema = !string.IsNullOrWhiteSpace(nimvoxDbContextOptions?.SchemaName)
            ? nimvoxDbContextOptions.SchemaName
            : NimvoxSchema;
    }

    /// <summary>
    ///     The default schema used by Nimvox.
    /// </summary>
    public static string NimvoxSchema { get; set; } = "Nimvox";

    /// <summary>
    ///     The table used to store the migrations history.
    /// </summary>
    public static string MigrationsHistoryTable { get; } = "__EFMigrationsHistory";

    /// <inheritdoc />
    public string Schema { get; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (!string.IsNullOrWhiteSpace(Schema))
            if (!Database.IsSqlite())
                modelBuilder.HasDefaultSchema(Schema);

        ApplyEntityConfigurations(modelBuilder);

        if (Database.IsSqlite()) SetupForSqlite(modelBuilder);
    }

    /// <summary>
    ///     Override this method to apply entity configurations.
    /// </summary>
    protected virtual void ApplyEntityConfigurations(ModelBuilder modelBuilder)
    {
    }

    /// <summary>
    ///     Override this method to apply entity configurations.
    /// </summary>
    protected virtual void Configure(ModelBuilder modelBuilder)
    {
    }

    /// <summary>
    ///     Override this method to apply entity configurations for the SQLite provider.
    /// </summary>
    protected virtual void SetupForSqlite(ModelBuilder modelBuilder)
    {
        // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
        // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties().Where(p =>
                p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));

            foreach (var property in properties)
                modelBuilder
                    .Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(new DateTimeOffsetToStringConverter());
        }
    }
}