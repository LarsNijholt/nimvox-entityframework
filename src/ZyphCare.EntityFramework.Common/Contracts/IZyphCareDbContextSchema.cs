namespace ZyphCare.EntityFramework.Common.Contracts;

/// <summary>
/// Interface to provide custom ZyphCare Db Context Schema in Migration
/// </summary>
public interface IZyphCareDbContextSchema
{
    /// <summary>
    ///  Name of the Schema
    /// </summary>
    public string Schema { get; }
}