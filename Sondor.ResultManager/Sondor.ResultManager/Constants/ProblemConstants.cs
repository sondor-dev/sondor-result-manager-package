namespace Sondor.ResultManager.Constants;

/// <summary>
/// Collection of problem constants.
/// </summary>
public class ProblemConstants
{
    /// <summary>
    /// The trace identifier key.
    /// </summary>
    public const string TraceKey = "traceId";

    /// <summary>
    /// The base problem type.
    /// </summary>
    public const string ProblemType = "https://support.sondor-technology.co.uk/problems";

    /// <summary>
    /// The conflict type.
    /// </summary>
    public const string ConflictType = $"{ProblemType}/conflict";

    /// <summary>
    /// The forbidden type.
    /// </summary>
    public const string ForbiddenType = $"{ProblemType}/forbidden";

    /// <summary>
    /// The bad request type.
    /// </summary>
    public const string BadRequestType = $"{ProblemType}/bad-request";

    /// <summary>
    /// The unauthorized type.
    /// </summary>
    public const string UnauthorizedType = $"{ProblemType}/unauthorized";

    /// <summary>
    /// The unexpected error type.
    /// </summary>
    public const string UnexpectedErrorType = $"{ProblemType}/unexpected-error";

    /// <summary>
    /// The request cancelled type.
    /// </summary>
    public const string RequestCancelledType = $"{ProblemType}/request-cancelled";

    /// <summary>
    /// The not found type.
    /// </summary>
    public const string ResourceNotFoundType = $"{ProblemType}/resource-not-found";

    /// <summary>
    /// The resource patch failed type.
    /// </summary>
    public const string ResourcePatchFailedType = $"{ProblemType}/resource-patch-failed";

    /// <summary>
    /// The resource delete failed type.
    /// </summary>
    public const string ResourceDeleteFailedType = $"{ProblemType}/resource-delete-failed";

    /// <summary>
    /// The resource update failed type.
    /// </summary>
    public const string ResourceUpdateFailedType = $"{ProblemType}/resource-update-failed";

    /// <summary>
    /// The resource create failed type.
    /// </summary>
    public const string ResourceCreateFailedType = $"{ProblemType}/resource-creation-failed";

    /// <summary>
    /// The bad request title.
    /// </summary>
    public const string BadRequestTitle = "Bad request!";

    /// <summary>
    /// The unauthorized title.
    /// </summary>
    public const string UnauthorizedTitle = "Unauthorized!";

    /// <summary>
    /// The forbidden title.
    /// </summary>
    public const string ForbiddenTitle = "Permission denied!";

    /// <summary>
    /// The conflict title.
    /// </summary>
    public const string ConflictTitle = "Resource already exists!";

    /// <summary>
    /// The unexpected error title.
    /// </summary>
    public const string UnexpectedErrorTitle = "Unexpected error!";

    /// <summary>
    /// The request cancelled title.
    /// </summary>
    public const string RequestCancelledTitle = "Request cancelled!";

    /// <summary>
    /// The not found title.
    /// </summary>
    public const string ResourceNotFoundTitle = "Resource not found!";

    /// <summary>
    /// The resource patch failed title.
    /// </summary>
    public const string ResourcePatchFailedTitle = "Resource patch failed!";

    /// <summary>
    /// The resource update failed title.
    /// </summary>
    public const string ResourceUpdateFailedTitle = "Resource update failed!";

    /// <summary>
    /// The resource delete failed title.
    /// </summary>
    public const string ResourceDeleteFailedTitle = "Resource delete failed!";

    /// <summary>
    /// The resource create failed title.
    /// </summary>
    public const string ResourceCreateFailedTitle = "Resource creation failed!";
}