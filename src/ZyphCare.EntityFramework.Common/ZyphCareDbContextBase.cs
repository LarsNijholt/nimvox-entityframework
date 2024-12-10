using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZyphCare.EntityFramework.Common.Contracts;

namespace ZyphCare.EntityFramework.Common;

public abstract class ZyphCareDbContextBase : DbContext, IZyphCareDbContextSchema
{
    private static readonly ISet<EntityState> ModifiedEntityStates = new HashSet<EntityState>
    {
        EntityState.Added,
        EntityState.Modified
    };

    /// <summary>
    ///     Initializes a new instance of the <see cref="ZyphCareDbContextBase" /> class.
    /// </summary>
    protected ZyphCareDbContextBase(DbContextOptions options) : base(options)
    {
        var ZyphCareDbContextOptions = options.FindExtension<ZyphCareDbContextOptionsExtension>()?.Options;

        Schema = !string.IsNullOrWhiteSpace(ZyphCareDbContextOptions?.SchemaName)
            ? ZyphCareDbContextOptions.SchemaName
            : ZyphCareSchema;
    }

    /// <summary>
    ///     The default schema used by ZyphCare.
    /// </summary>
    public static string ZyphCareSchema { get; set; } = "ZyphCare";

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