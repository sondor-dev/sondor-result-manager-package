using FluentValidation.Results;
using Sondor.Errors;

namespace Sondor.ResultManager.Extensions;

/// <summary>
/// Collection of <see cref="ISondorResultManager"/>.
/// </summary>
public static class SondorResultManagerExtensions
{
    /// <summary>
    /// Success result.
    /// </summary>
    /// <param name="_">The result manager.</param>
    /// <returns>Returns the result.</returns>
    public static SondorResult Success(this ISondorResultManager _)
    {
        return new SondorResult();
    }

    /// <summary>
    /// Success result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="_">The result manager.</param>
    /// <param name="result">The result</param>
    /// <returns>Returns the result.</returns>
    public static SondorResult<TResult> Success<TResult>(this ISondorResultManager _,
        TResult result)
    {
        return new SondorResult<TResult>(result);
    }

    /// <summary>
    /// Bad request result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="failures">The validation failures.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the bad request result.</returns>
    public static async Task<SondorResult> BadRequest(this ISondorResultManager resultManager,
        IEnumerable<ValidationFailure> failures,
        CancellationToken cancellationToken = default)
    {
        var failuresList = failures.ToList();

        var errorFormat = await resultManager.GetErrorFormat(SondorErrorCodes.BadRequest, cancellationToken);
        var description = string.Format(errorFormat, failuresList.Count);
        var error = new SondorError(SondorErrorCodes.BadRequest,
            SondorErrorTypes.BadRequestType,
            description);

        error.Context.Add("Errors", failuresList);

        return new SondorResult(error);
    }

    /// <summary>
    /// Resource not found result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="failures">The validation failures.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource not found result.</returns>
    public static async Task<SondorResult<TResult>> BadRequest<TResult>(this ISondorResultManager resultManager,
        IEnumerable<ValidationFailure> failures,
        CancellationToken cancellationToken = default)
    {
        var result = await BadRequest(resultManager, failures, cancellationToken);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource already exists result.
    /// </summary>
    /// <param name="entity">The entity name.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyValue">The property value.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource already exists result.</returns>
    public static async Task<SondorResult> ResourceAlreadyExists(this ISondorResultManager resultManager,
        string entity,
        string propertyName,
        string propertyValue,
        CancellationToken cancellationToken = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException($"{nameof(entity)} cannot be null.", nameof(entity));
        }

        if (string.IsNullOrWhiteSpace(entity))
        {
            throw new ArgumentException($"{nameof(entity)} cannot be empty or whitespace.", nameof(entity));
        }

        if (propertyName is null)
        {
            throw new ArgumentNullException($"{nameof(propertyName)} cannot be null.", nameof(propertyName));
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException($"{nameof(propertyName)} empty or whitespace.", nameof(propertyName));
        }

        if (propertyValue is null)
        {
            throw new ArgumentNullException($"{nameof(propertyValue)} cannot be null.", nameof(propertyValue));
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            throw new ArgumentException($"{nameof(propertyValue)} cannot be empty or whitespace.", nameof(propertyValue));
        }

        var format = await resultManager.GetErrorFormat(SondorErrorCodes.ResourceAlreadyExists, cancellationToken);
        var description = string.Format(format, entity, propertyName, propertyValue);
        var error = new SondorError(SondorErrorCodes.ResourceAlreadyExists,
            SondorErrorTypes.ResourceAlreadyExistsType,
            description);

        return new SondorResult(error);
    }

    /// <summary>
    /// Resource already exists result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="entity">The entity name.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyValue">The property value.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource already exists result.</returns>
    public static async Task<SondorResult<TResult>> ResourceAlreadyExists<TResult>(this ISondorResultManager resultManager,
        string entity,
        string propertyName,
        string propertyValue,
        CancellationToken cancellationToken = default)
    {
        var result = await ResourceAlreadyExists(resultManager,
            entity,
            propertyName,
            propertyValue,
            cancellationToken);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource create failed result.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="resource">The resource.</param>
    /// <param name="resourceName">The resource name.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource create failed result.</returns>
    public static async Task<SondorResult> ResourceCreateFailed<TResource>(this ISondorResultManager resultManager,
        string resourceName,
        TResource resource,
        CancellationToken cancellationToken = default)
    {
        if (resourceName is null)
        {
            throw new ArgumentNullException($"{nameof(resourceName)} cannot be null.", nameof(resourceName));
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            throw new ArgumentException($"{nameof(resourceName)} cannot be empty or whitespace.", nameof(resourceName));
        }

        var format = await resultManager.GetErrorFormat(SondorErrorCodes.ResourceCreateFailed, cancellationToken);
        var description = string.Format(format, resourceName);
        var error = new SondorError(SondorErrorCodes.ResourceCreateFailed,
            SondorErrorTypes.ResourceCreateFailedType,
            description);

        error.Context.Add(resourceName, resource);

        return new SondorResult(error);
    }

    /// <summary>
    /// Resource create failed result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="resource">The resource.</param>
    /// <param name="resourceName">The resource name.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource create failed result.</returns>
    public static async Task<SondorResult<TResult>> ResourceCreateFailed<TResource, TResult>(this ISondorResultManager resultManager,
        string resourceName,
        TResource resource,
        CancellationToken cancellationToken = default)
    {
        var result = await ResourceCreateFailed(resultManager,
            resourceName,
            resource,
            cancellationToken);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource update failed result.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="resource">The resource.</param>
    /// <param name="resourceName">The resource name.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource update failed result.</returns>
    public static async Task<SondorResult> ResourceUpdateFailed<TResource>(this ISondorResultManager resultManager,
        string resourceName,
        TResource resource,
        CancellationToken cancellationToken = default)
    {
        if (resourceName is null)
        {
            throw new ArgumentNullException($"{nameof(resourceName)} cannot be null.", nameof(resourceName));
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            throw new ArgumentException($"{nameof(resourceName)} cannot be empty or whitespace.", nameof(resourceName));
        }

        var format = await resultManager.GetErrorFormat(SondorErrorCodes.ResourceUpdateFailed, cancellationToken);
        var description = string.Format(format, resourceName);
        var error = new SondorError(SondorErrorCodes.ResourceUpdateFailed,
            SondorErrorTypes.ResourceUpdateFailedType,
            description);

        error.Context.Add(resourceName, resource);

        return new SondorResult(error);
    }

    /// <summary>
    /// Resource update failed result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="resource">The resource.</param>
    /// <param name="resourceName">The resource name.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource update failed result.</returns>
    public static async Task<SondorResult<TResult>> ResourceUpdateFailed<TResource, TResult>(this ISondorResultManager resultManager,
        string resourceName,
        TResource resource,
        CancellationToken cancellationToken = default)
    {
        var result = await ResourceUpdateFailed(resultManager,
            resourceName,
            resource,
            cancellationToken);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource delete failed result.
    /// </summary>
    /// <param name="resource">The resource name.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource delete failed result.</returns>
    public static async Task<SondorResult> ResourceDeleteFailed(this ISondorResultManager resultManager,
        string resource,
        CancellationToken cancellationToken = default)
    {
        if (resource is null)
        {
            throw new ArgumentNullException($"{nameof(resource)} cannot be null.", nameof(resource));
        }

        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException($"{nameof(resource)} cannot be empty or whitespace.", nameof(resource));
        }

        var format = await resultManager.GetErrorFormat(SondorErrorCodes.ResourceDeleteFailed, cancellationToken);
        var description = string.Format(format, resource);
        var error = new SondorError(SondorErrorCodes.ResourceDeleteFailed,
            SondorErrorTypes.ResourceDeleteFailedType,
            description);

        return new SondorResult(error);
    }

    /// <summary>
    /// Resource delete failed result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="resource">The resource name.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource delete failed result.</returns>
    public static async Task<SondorResult<TResult>> ResourceDeleteFailed<TResult>(this ISondorResultManager resultManager,
        string resource,
        CancellationToken cancellationToken = default)
    {
        var result = await ResourceDeleteFailed(resultManager,
            resource,
            cancellationToken);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource not found result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="entity">The entity name.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyValue">The property name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource not found result.</returns>
    public static async Task<SondorResult> ResourceNotFound(this ISondorResultManager resultManager,
        string entity,
        string propertyName,
        string propertyValue,
        CancellationToken cancellationToken = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException($"{nameof(entity)} cannot be null.", nameof(entity));
        }

        if (string.IsNullOrWhiteSpace(entity))
        {
            throw new ArgumentException($"{nameof(entity)} cannot be empty or whitespace.", nameof(entity));
        }

        if (propertyName is null)
        {
            throw new ArgumentNullException($"{nameof(propertyName)} cannot be null.", nameof(propertyName));
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException($"{nameof(propertyName)} empty or whitespace.", nameof(propertyName));
        }

        if (propertyValue is null)
        {
            throw new ArgumentNullException($"{nameof(propertyValue)} cannot be null.", nameof(propertyValue));
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            throw new ArgumentException($"{nameof(propertyValue)} cannot be empty or whitespace.", nameof(propertyValue));
        }

        var errorFormat = await resultManager.GetErrorFormat(SondorErrorCodes.ResourceNotFound, cancellationToken);
        var description = string.Format(errorFormat, entity, propertyName, propertyValue);
        var error = new SondorError(SondorErrorCodes.ResourceNotFound,
            SondorErrorTypes.ResourceNotFoundType,
            description);

        return new SondorResult(error);
    }

    /// <summary>
    /// Resource not found result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="entity">The entity name.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyValue">The property name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource not found result.</returns>
    public static async Task<SondorResult<TResult>> ResourceNotFound<TResult>(this ISondorResultManager resultManager,
        string entity,
        string propertyName,
        string propertyValue,
        CancellationToken cancellationToken = default)
    {
        var result = await ResourceNotFound(resultManager,
            entity,
            propertyName,
            propertyValue,
            cancellationToken);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource not found result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="message">The error message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource not found result.</returns>
    public static async Task<SondorResult> UnexpectedError(this ISondorResultManager resultManager,
        string message,
        CancellationToken cancellationToken = default)
    {
        if (message is null)
        {
            throw new ArgumentNullException($"{nameof(message)} cannot be null.", nameof(message));
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException($"{nameof(message)} cannot be empty or whitespace.", nameof(message));
        }

        var errorFormat = await resultManager.GetErrorFormat(SondorErrorCodes.UnexpectedError, cancellationToken);
        var description = string.Format(errorFormat, message);
        var error = new SondorError(SondorErrorCodes.UnexpectedError,
            SondorErrorTypes.UnexpectedErrorType,
            description);

        return new SondorResult(error);
    }

    /// <summary>
    /// Resource not found result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="message">The error message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns the resource not found result.</returns>
    public static async Task<SondorResult<TResult>> UnexpectedError<TResult>(this ISondorResultManager resultManager,
        string message,
        CancellationToken cancellationToken = default)
    {
        var result = await UnexpectedError(resultManager, message, cancellationToken);

        return new SondorResult<TResult>(result.Error);
    }
}
