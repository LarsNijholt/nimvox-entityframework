using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nimvox.EntityFramework.Common;

/// <summary>
/// Provides options for configuring Nimvox's Entity Framework Core integration.
/// </summary>
public static class NimvoxDbContextOptionsExtensions
{
    /// <summary>
    /// Installs a custom extension for Nimvox's Entity Framework Core integration.
    /// </summary>
    /// <param name="optionsBuilder">The options builder to install the extension on.</param>
    /// <param name="options">The options to install.</param>
    public static DbContextOptionsBuilder UseNimvoxDbContextOptions(this DbContextOptionsBuilder optionsBuilder, NimvoxDbContextOptions? options)
    {
        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(new NimvoxDbContextOptionsExtension(options));
        optionsBuilder.ReplaceService<IMigrationsAssembly, DbSchemaAwareMigrationAssembly>();
        return optionsBuilder;
    }

    public static string GetMigrationsAssemblyName(this NimvoxDbContextOptions? options, Assembly migrationsAssembly) => options?.MigrationsAssemblyName ?? migrationsAssembly.GetName().Name!;
    
    public static string GetMigrationsHistoryTableName(this NimvoxDbContextOptions? options) => options?.MigrationsHistoryTableName ?? NimvoxDbContextBase.MigrationsHistoryTable;
    
    public static string GetSchemaName(this NimvoxDbContextOptions? options) => options?.SchemaName ?? NimvoxDbContextBase.NimvoxSchema;
}