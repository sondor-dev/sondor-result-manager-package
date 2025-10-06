using System.Text.Json;
using FluentValidation.Results;
using Sondor.Errors;
using Sondor.ProblemResults.Constants;
using Sondor.ProblemResults.Extensions;

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
    /// <param name="errorMessage">The error message.</param>
    /// <returns>Returns the bad request result.</returns>
    public static SondorResult BadRequest(this ISondorResultManager resultManager,
        string errorMessage)
    {
        return new SondorResult(new SondorError(SondorErrorCodes.BadRequest,
            ProblemResultConstants.FindProblemTypeByErrorCode(SondorErrorCodes.BadRequest),
            errorMessage,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.BadRequest },
                { ProblemResultConstants.ErrorMessage, errorMessage }
            }));
    }

    /// <inheritdoc cref="BadRequest"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    public static SondorResult<TResult> BadRequest<TResult>(this ISondorResultManager resultManager,
        string errorMessage)
    {
        var result = BadRequest(resultManager, errorMessage);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Validation result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="failures">The validation failures.</param>
    /// <returns>Returns the validation result.</returns>
    public static SondorResult Validation(this ISondorResultManager resultManager,
        IEnumerable<ValidationFailure> failures)
    {
        var failuresList = failures.ToList();
        var errorMessage =
            resultManager.TranslationManager.ProblemValidationErrors(failuresList.Count);

        return new SondorResult(new SondorError(SondorErrorCodes.ValidationFailed,
            ProblemResultConstants.FindProblemTypeByErrorCode(SondorErrorCodes.ValidationFailed),
            errorMessage,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ValidationFailed },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Errors, failuresList }
            }));
    }

    /// <inheritdoc cref="Validation"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    public static SondorResult<TResult> Validation<TResult>(this ISondorResultManager resultManager,
        IEnumerable<ValidationFailure> failures)
    {
        var result = Validation(resultManager, failures);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource already exists result.
    /// </summary>
    /// <param name="entity">The entity name.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyValue">The property value.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <returns>Returns the resource already exists result.</returns>
    public static SondorResult ResourceAlreadyExists(this ISondorResultManager resultManager,
        string entity,
        string propertyName,
        string propertyValue)
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

        var errorMessage =
            resultManager.TranslationManager.ProblemResourceAlreadyExists(entity, propertyName, propertyValue);

        return new SondorResult(new SondorError(SondorErrorCodes.ResourceAlreadyExists,
            ProblemResultConstants.FindProblemTypeByErrorCode(SondorErrorCodes.ResourceAlreadyExists),
            errorMessage,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceAlreadyExists },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Resource, entity },
                { ProblemResultConstants.PropertyName, propertyName },
                { ProblemResultConstants.PropertyValue, propertyValue }
            }));
    }

    /// <inheritdoc cref="ResourceAlreadyExists"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    public static SondorResult<TResult> ResourceAlreadyExists<TResult>(this ISondorResultManager resultManager,
        string entity,
        string propertyName,
        string propertyValue)
    {
        var result = ResourceAlreadyExists(resultManager,
            entity,
            propertyName,
            propertyValue);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource create failed result.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="resource">The resource.</param>
    /// <param name="resourceName">The resource name.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <returns>Returns the resource create failed result.</returns>
    public static SondorResult ResourceCreateFailed<TResource>(this ISondorResultManager resultManager,
        string resourceName,
        TResource resource)
    {
        if (resourceName is null)
        {
            throw new ArgumentNullException($"{nameof(resourceName)} cannot be null.", nameof(resourceName));
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            throw new ArgumentException($"{nameof(resourceName)} cannot be empty or whitespace.", nameof(resourceName));
        }

        var errorDescription = resultManager.TranslationManager.ProblemResourceCreateFailed(resourceName);

        return new SondorResult(new SondorError(SondorErrorCodes.ResourceCreateFailed,
            ProblemResultConstants.FindProblemTypeByErrorCode(SondorErrorCodes.ResourceCreateFailed),
            errorDescription,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext?.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceCreateFailed },
                { ProblemResultConstants.ErrorMessage, errorDescription },
                { ProblemResultConstants.Resource, resource },
                { ProblemResultConstants.NewResource, JsonSerializer.Serialize(resource) }
            }));
    }

    /// <inheritdoc cref="ResourceCreateFailed{TResource}"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TResource">The resource type.</typeparam>
    public static SondorResult<TResult> ResourceCreateFailed<TResource, TResult>(this ISondorResultManager resultManager,
        string resourceName,
        TResource resource)
    {
        var result = ResourceCreateFailed(resultManager,
            resourceName,
            resource);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource update failed result.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="resource">The resource.</param>
    /// <param name="resourceName">The resource name.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="reasons">The reasons the update failed.</param>
    /// <param name="updatedResource">The updated resource.</param>
    /// <returns>Returns the resource update failed result.</returns>
    public static SondorResult ResourceUpdateFailed<TResource>(this ISondorResultManager resultManager,
        string resourceName,
        TResource? updatedResource,
        TResource? resource = default,
        string[]? reasons = null)
    {
        if (resourceName is null)
        {
            throw new ArgumentNullException($"{nameof(resourceName)} cannot be null.", nameof(resourceName));
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            throw new ArgumentException($"{nameof(resourceName)} cannot be empty or whitespace.", nameof(resourceName));
        }

        reasons ??= [];
        var errorMessage = resultManager.TranslationManager.ProblemResourceUpdateFailed(resourceName);

        return new SondorResult(new SondorError(SondorErrorCodes.ResourceUpdateFailed,
            ProblemResultConstants.FindProblemTypeByErrorCode(SondorErrorCodes.ResourceUpdateFailed),
            errorMessage,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceUpdateFailed },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Reasons, reasons },
                { ProblemResultConstants.Resource, resource },
                { ProblemResultConstants.UpdatedResource, updatedResource }
            }));
    }

    /// <inheritdoc cref="ResourceUpdateFailed{TResource}" />
    /// <typeparam name="TResult">the result type.</typeparam>
    /// <typeparam name="TResource">The resource type.</typeparam>
    public static SondorResult<TResult> ResourceUpdateFailed<TResource, TResult>(this ISondorResultManager resultManager,
        string resourceName,
        TResource? updatedResource,
        TResource? resource = default,
        string[]? reasons = null)
    {
        var result = ResourceUpdateFailed(resultManager,
            resourceName,
            updatedResource,
            resource,
            reasons);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource delete failed result.
    /// </summary>
    /// <param name="resource">The resource name.</param>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="reasons">The reasons the deletion failed.</param>
    /// <returns>Returns the resource delete failed result.</returns>
    public static SondorResult ResourceDeleteFailed(this ISondorResultManager resultManager,
        string resource,
        string[]? reasons = null)
    {
        if (resource is null)
        {
            throw new ArgumentNullException($"{nameof(resource)} cannot be null.", nameof(resource));
        }

        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException($"{nameof(resource)} cannot be empty or whitespace.", nameof(resource));
        }

        var errorMessage = resultManager.TranslationManager.ProblemResourceDeleteFailed(resource);

        return new SondorResult(new SondorError(SondorErrorCodes.ResourceDeleteFailed,
            ProblemResultConstants.FindProblemTypeByErrorCode(SondorErrorCodes.ResourceDeleteFailed),
            errorMessage,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceDeleteFailed },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Reasons, reasons },
                { ProblemResultConstants.Resource, resource }
            }));
    }

    /// <inheritdoc cref="ResourceDeleteFailed"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    public static SondorResult<TResult> ResourceDeleteFailed<TResult>(this ISondorResultManager resultManager,
        string resource,
        string[]? reasons = null)
    {
        var result = ResourceDeleteFailed(resultManager,
            resource,
            reasons);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Resource not found result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="entity">The entity name.</param>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyValue">The property name.</param>
    /// <returns>Returns the resource not found result.</returns>
    public static SondorResult ResourceNotFound(this ISondorResultManager resultManager,
        string entity,
        string propertyName,
        string propertyValue)
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

        var errorMessage = resultManager.TranslationManager.ProblemResourceNotFound(entity, propertyName, propertyValue);

        return new SondorResult(new SondorError(SondorErrorCodes.ResourceNotFound,
            ProblemResultConstants.FindProblemTypeByErrorCode(SondorErrorCodes.ResourceNotFound),
            errorMessage,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceNotFound },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Resource, entity },
                { ProblemResultConstants.PropertyName, propertyName },
                { ProblemResultConstants.PropertyValue, propertyValue }
            }));
    }

    /// <inheritdoc cref="ResourceNotFound"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    public static SondorResult<TResult> ResourceNotFound<TResult>(this ISondorResultManager resultManager,
        string entity,
        string propertyName,
        string propertyValue)
    {
        var result = ResourceNotFound(resultManager,
            entity,
            propertyName,
            propertyValue);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Unexpected error result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="message">The error message.</param>
    /// <returns>Returns unexpected error result.</returns>
    public static SondorResult UnexpectedError(this ISondorResultManager resultManager,
        string message)
    {
        if (message is null)
        {
            throw new ArgumentNullException($"{nameof(message)} cannot be null.", nameof(message));
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException($"{nameof(message)} cannot be empty or whitespace.", nameof(message));
        }

        var errorMessage = resultManager.TranslationManager.ProblemUnexpectedError();

        return new SondorResult(new SondorError(SondorErrorCodes.UnexpectedError,
            SondorErrorTypes.UnexpectedErrorType,
            errorMessage,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.UnexpectedError },
                { ProblemResultConstants.ErrorMessage, message }
            }));
    }

    /// <inheritdoc cref="UnexpectedError"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    public static SondorResult<TResult> UnexpectedError<TResult>(this ISondorResultManager resultManager,
        string message)
    {
        var result = UnexpectedError(resultManager, message);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Unauthorized result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>Returns the unauthorized result.</returns>
    public static SondorResult Unauthorized(this ISondorResultManager resultManager,
        string resource)
    {
        if (resource is null)
        {
            throw new ArgumentNullException($"{nameof(resource)} cannot be null.", nameof(resource));
        }

        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new ArgumentException($"{nameof(resource)} cannot be empty or whitespace.", nameof(resource));
        }

        var errorMessage = resultManager.TranslationManager.ProblemUnauthorized(resource);

        return new SondorResult(new SondorError(SondorErrorCodes.Unauthorized,
            ProblemResultConstants.UnauthorizedType,
            errorMessage,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.Unauthorized },
                { ProblemResultConstants.ErrorMessage, errorMessage },
                { ProblemResultConstants.Resource, resource }
            }));
    }

    /// <inheritdoc cref="Unauthorized"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    public static SondorResult<TResult> Unauthorized<TResult>(this ISondorResultManager resultManager,
        string resource)
    {
        var result = Unauthorized(resultManager, resource);

        return new SondorResult<TResult>(result.Error);
    }

    /// <summary>
    /// Forbidden result.
    /// </summary>
    /// <param name="resultManager">The result manager.</param>
    /// <returns>Returns the forbidden result.</returns>
    public static SondorResult Forbidden(this ISondorResultManager resultManager)
    {
        var errorMessage = resultManager.TranslationManager.ProblemForbidden();

        return new SondorResult(new SondorError(SondorErrorCodes.Forbidden,
            ProblemResultConstants.ForbiddenType,
            errorMessage,
            new Dictionary<string, object?>
            {
                { ProblemResultConstants.TraceKey, resultManager.HttpContextAccessor.HttpContext.TraceIdentifier },
                { ProblemResultConstants.ErrorCode, SondorErrorCodes.Forbidden },
                { ProblemResultConstants.ErrorMessage, errorMessage }
            }));
    }

    /// <inheritdoc cref="Forbidden"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    public static SondorResult<TResult> Forbidden<TResult>(this ISondorResultManager resultManager)
    {
        var result = Forbidden(resultManager);

        return new SondorResult<TResult>(result.Error);
    }
}
