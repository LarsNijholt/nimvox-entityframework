using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZyphCare.EntityFramework.Common;

/// <summary>
///     Provides options for configuring ZyphCare's Entity Framework Core integration.
/// </summary>
public static class ZyphCareDbContextOptionsExtensions
{
    /// <summary>
    ///     Installs a custom extension for ZyphCare's Entity Framework Core integration.
    /// </summary>
    /// <param name="optionsBuilder">The options builder to install the extension on.</param>
    /// <param name="options">The options to install.</param>
    public static DbContextOptionsBuilder UseZyphCareDbContextOptions(this DbContextOptionsBuilder optionsBuilder,
        ZyphCareDbContextOptions? options)
    {
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(
            new ZyphCareDbContextOptionsExtension(options));
        optionsBuilder.ReplaceService<IMigrationsAssembly, DbSchemaAwareMigrationAssembly>();
        return optionsBuilder;
    }

    public static string GetMigrationsAssemblyName(this ZyphCareDbContextOptions? options, Assembly migrationsAssembly)
    {
        return options?.MigrationsAssemblyName ?? migrationsAssembly.GetName().Name!;
    }

    public static string GetMigrationsHistoryTableName(this ZyphCareDbContextOptions? options)
    {
        return options?.MigrationsHistoryTableName ?? ZyphCareDbContextBase.MigrationsHistoryTable;
    }

    public static string GetSchemaName(this ZyphCareDbContextOptions? options)
    {
        return options?.SchemaName ?? ZyphCareDbContextBase.ZyphCareSchema;
    }
}