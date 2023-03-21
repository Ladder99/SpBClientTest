// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A class that contains constant values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SparkplugNet.Core;

/// <summary>
/// A class that contains constant values.
/// </summary>
public static class Constants
{
    /// <summary>
    /// The session number metric name.
    /// </summary>
    public const string SessionNumberMetricName = "BDSEQ";

    /// <summary>
    /// The epoch.
    /// </summary>
    public static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
}
