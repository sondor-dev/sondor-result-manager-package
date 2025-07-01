using Sondor.Errors;

namespace Sondor.ResultManager;

/// <summary>
/// The marker interface, for <see cref="SondorResult"/> management.
/// </summary>
public interface ISondorResultManager
{
    /// <summary>
    /// Gets the error type.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <returns>Returns the error type.</returns>
    string GetErrorType(int errorCode);

    /// <summary>
    /// Gets the error format.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the error format.</returns>
    Task<string> GetErrorFormat(int errorCode,
        CancellationToken cancellationToken = default);
}