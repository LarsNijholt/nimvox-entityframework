namespace ZyphCare.EntityFramework.Common;

/// <summary>
///     Provides options for configuring ZyphCare's Entity Framework Core integration.
/// </summary>
public class ZyphCareDbContextOptions
{
    /// <summary>
    ///     The schema used by ZyphCare.
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