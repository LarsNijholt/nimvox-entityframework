namespace Nimvox.EntityFramework.Common.Contracts;

/// <summary>
/// Interface to provide custom Nimvox Db Context Schema in Migration
/// </summary>
public interface INimvoxDbContextSchema
{
    /// <summary>
    /// Name of the Schema
    /// </summary>
    public string Schema { get; }
}