using Sondor.Errors;
using Sondor.Errors.Tests.Args;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Sondor.ResultManager.Extensions;

namespace Sondor.ResultManager.Tests;

/// <summary>
/// Tests for <see cref="SondorResultManager"/>.
/// </summary>
[TestFixture]
public class SondorResultManagerTests
{
    /// <summary>
    /// The result manager.
    /// </summary>
    private readonly ISondorResultManager _resultManager;

    /// <summary>
    /// Creates a new instance of <see cref="SondorResultManagerTests"/>.
    /// </summary>
    public SondorResultManagerTests()
    {
        var services = new ServiceCollection();
        services.AddSondorResultManager();

        var serviceProvider = services.BuildServiceProvider();
        _resultManager = serviceProvider.GetRequiredService<ISondorResultManager>();
    }

    /// <summary>
    /// Ensures that <see cref="ISondorResultManager.GetErrorType"/> works as expected.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    [TestCaseSource(typeof(SondorErrorCodeArgs))]
    [ExcludeFromCodeCoverage]
    public void GetErrorType(int errorCode)
    {
        // assert
        var expected = errorCode switch
        {
            SondorErrorCodes.ResourceNotFound => SondorErrorTypes.ResourceNotFoundType,
            SondorErrorCodes.ResourceCreateFailed => SondorErrorTypes.ResourceCreateFailedType,
            SondorErrorCodes.ResourceUpdateFailed => SondorErrorTypes.ResourceUpdateFailedType,
            SondorErrorCodes.ResourceDeleteFailed => SondorErrorTypes.ResourceDeleteFailedType,
            SondorErrorCodes.ResourcePatchFailed => SondorErrorTypes.ResourcePatchFailedType,
            SondorErrorCodes.ResourceAlreadyExists => SondorErrorTypes.ResourceAlreadyExistsType,
            SondorErrorCodes.Unauthorized => SondorErrorTypes.UnauthorizedType,
            SondorErrorCodes.BadRequest => SondorErrorTypes.BadRequestType,
            SondorErrorCodes.Forbidden => SondorErrorTypes.ForbiddenType,
            SondorErrorCodes.UnexpectedError => SondorErrorTypes.UnexpectedErrorType,
            _ => throw new AssertionException($"Missing error code! Error code: {errorCode}36")
        };

        // act
        var errorType = _resultManager.GetErrorType(errorCode);

        // assert
        Assert.That(errorType, Is.EqualTo(expected));
    }

    /// <summary>
    /// Ensures that <see cref="ISondorResultManager.GetErrorFormat(int, CancellationToken)"/> works as expected.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    [TestCaseSource(typeof(SondorErrorCodeArgs))]
    [ExcludeFromCodeCoverage]
    public async Task GetErrorFormat(int errorCode)
    {
        // assert
        var expected = errorCode switch
        {
            SondorErrorCodes.ResourceNotFound => SondorErrorMessages.ResourceNotFound,
            SondorErrorCodes.ResourceCreateFailed => SondorErrorMessages.ResourceCreateFailed,
            SondorErrorCodes.ResourceUpdateFailed => SondorErrorMessages.ResourceUpdateFailed,
            SondorErrorCodes.ResourceDeleteFailed => SondorErrorMessages.ResourceDeleteFailed,
            SondorErrorCodes.ResourcePatchFailed => SondorErrorMessages.ResourcePatchFailed,
            SondorErrorCodes.ResourceAlreadyExists => SondorErrorMessages.ResourceAlreadyExists,
            SondorErrorCodes.Unauthorized => SondorErrorMessages.Unauthorized,
            SondorErrorCodes.BadRequest => SondorErrorMessages.BadRequest,
            SondorErrorCodes.Forbidden => SondorErrorMessages.Forbidden,
            SondorErrorCodes.UnexpectedError => SondorErrorMessages.UnexpectedError,
            _ => throw new AssertionException($"Missing error code! Error code: {errorCode}36")
        };

        // act
        var errorFormat = await _resultManager.GetErrorFormat(errorCode, TestContext.CurrentContext.CancellationToken);

        // assert
        Assert.That(errorFormat, Is.EqualTo(expected));
    }
}