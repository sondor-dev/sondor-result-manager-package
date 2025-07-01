using Sondor.Errors;

namespace Sondor.ResultManager;

/// <summary>
/// The <see cref="Errors.SondorResult"/> manager.
/// </summary>
public class SondorResultManager :
    ISondorResultManager
{
    /// <inheritdoc />
    public virtual string GetErrorType(int errorCode)
    {
        return errorCode switch
        {
            SondorErrorCodes.BadRequest => SondorErrorTypes.BadRequestType,
            SondorErrorCodes.Forbidden => SondorErrorTypes.ForbiddenType,
            SondorErrorCodes.ResourceNotFound => SondorErrorTypes.ResourceNotFoundType,
            SondorErrorCodes.ResourceUpdateFailed => SondorErrorTypes.ResourceUpdateFailedType,
            SondorErrorCodes.ResourcePatchFailed => SondorErrorTypes.ResourcePatchFailedType,
            SondorErrorCodes.ResourceDeleteFailed => SondorErrorTypes.ResourceDeleteFailedType,
            SondorErrorCodes.ResourceCreateFailed => SondorErrorTypes.ResourceCreateFailedType,
            SondorErrorCodes.ResourceAlreadyExists => SondorErrorTypes.ResourceAlreadyExistsType,
            SondorErrorCodes.Unauthorized => SondorErrorTypes.UnauthorizedType,
            _ => SondorErrorTypes.UnexpectedErrorType
        };
    }

    /// <inheritdoc />
    public virtual Task<string> GetErrorFormat(int errorCode,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(errorCode switch
        {
            SondorErrorCodes.BadRequest => SondorErrorMessages.BadRequest,
            SondorErrorCodes.Forbidden => SondorErrorMessages.Forbidden,
            SondorErrorCodes.ResourceNotFound => SondorErrorMessages.ResourceNotFound,
            SondorErrorCodes.ResourceUpdateFailed => SondorErrorMessages.ResourceUpdateFailed,
            SondorErrorCodes.ResourcePatchFailed => SondorErrorMessages.ResourcePatchFailed,
            SondorErrorCodes.ResourceDeleteFailed => SondorErrorMessages.ResourceDeleteFailed,
            SondorErrorCodes.ResourceCreateFailed => SondorErrorMessages.ResourceCreateFailed,
            SondorErrorCodes.ResourceAlreadyExists => SondorErrorMessages.ResourceAlreadyExists,
            SondorErrorCodes.Unauthorized => SondorErrorMessages.Unauthorized,
            _ => SondorErrorMessages.UnexpectedError
        });
    }
}