namespace Nimvox.EntityFramework.Common;

/// <summary>
///     Provides options for configuring Nimvox's Entity Framework Core integration.
/// </summary>
public class NimvoxDbContextOptions
{
    /// <summary>
    ///     The schema used by Nimvox.
    /// </summary>
    public string? SchemaName { get; set; }

    /// <summary>
    ///     The table used to store the migrations history.
    /// </summary>
    public string? MigrationsHistoryTableName { get; set; }

    /// <summary>
    ///     The assembly name containing the migrations.
    /// </summary>
    public string? MigrationsAssemblyName { get; set; }
}