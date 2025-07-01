using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Sondor.Errors;
using Sondor.Errors.Tests;
using Sondor.ResultManager.Extensions;
using Sondor.Tests.Args;

namespace Sondor.ResultManager.Tests.Extensions;

/// <summary>
/// Tests for <see cref="SondorResultManagerExtensions"/>.
/// </summary>
[TestFixture]
public class SondorResultManagerExtensionsTests
{
    /// <summary>
    /// The result manager.
    /// </summary>
    private readonly ISondorResultManager _resultManager;

    /// <summary>
    /// Creates a new instance of <see cref="SondorResultManagerExtensionsTests"/>.
    /// </summary>
    public SondorResultManagerExtensionsTests()
    {
        var services = new ServiceCollection();
        services.AddSondorResultManager();

        var serviceProvider = services.BuildServiceProvider();
        _resultManager = serviceProvider.GetRequiredService<ISondorResultManager>();
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.BadRequest"/> works as intended.
    /// </summary>
    [Test]
    public async Task BadRequest()
    {
        // arrange
        var failures = new List<ValidationFailure>
        {
            new ("Id", "Invalid id", 0)
        };

        var description =
            string.Format(SondorErrorMessages.BadRequest, failures.Count);
        var error = new SondorError(SondorErrorCodes.BadRequest,
            SondorErrorTypes.BadRequestType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.BadRequest(failures,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.BadRequest"/> works as intended.
    /// </summary>
    [Test]
    public async Task BadRequest_typed()
    {
        // arrange
        var failures = new List<ValidationFailure>
        {
            new ("Id", "Invalid id", 0)
        };

        var description =
            string.Format(SondorErrorMessages.BadRequest, failures.Count);
        var error = new SondorError(SondorErrorCodes.BadRequest,
            SondorErrorTypes.BadRequestType,
            description);
        var expected = new SondorResult<int>(error);

        // act
        var result = await _resultManager.BadRequest<int>(failures,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> throws exceptions as expected.
    /// </summary>
    /// <param name="entityName">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceAlreadyExists_entity_exception(string? entityName)
    {
        // arrange
        const string propertyName = "Id";
        const string propertyValue = "1";

        // exceptions
        if (entityName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists(entityName!, propertyName, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> throws exceptions as expected.
    /// </summary>
    /// <param name="entityName">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceAlreadyExists_typed_entity_exception(string? entityName)
    {
        // arrange
        const string propertyName = "Id";
        const string propertyValue = "1";

        // exceptions
        if (entityName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists<int>(entityName!, propertyName, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> throws exceptions as expected.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceAlreadyExists_property_name_exception(string? propertyName)
    {
        // arrange
        const string entityName = "Entity";
        const string propertyValue = "1";

        // exceptions
        if (propertyName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName!, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> throws exceptions as expected.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceAlreadyExists_typed_property_name_exception(string? propertyName)
    {
        // arrange
        const string entityName = "Entity";
        const string propertyValue = "1";

        // exceptions
        if (propertyName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName!, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> throws exceptions as expected.
    /// </summary>
    /// <param name="propertyValue">The property value.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceAlreadyExists_property_value_exception(string? propertyValue)
    {
        // arrange
        const string entityName = "Entity";
        const string propertyName = "Id";

        // exceptions
        if (propertyValue is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue!));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> throws exceptions as expected.
    /// </summary>
    /// <param name="propertyValue">The property value.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceAlreadyExists_typed_property_value_exception(string? propertyValue)
    {
        // arrange
        const string entityName = "Entity";
        const string propertyName = "Id";

        // exceptions
        if (propertyValue is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue!));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceAlreadyExists()
    {
        // arrange
        const string entity = "Entity";
        const string propertyName = "Id";
        const string propertyValue = "1";

        var description =
            string.Format(SondorErrorMessages.ResourceAlreadyExists, entity, propertyName, propertyValue);
        var error = new SondorError(SondorErrorCodes.ResourceAlreadyExists,
            SondorErrorTypes.ResourceAlreadyExistsType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.ResourceAlreadyExists(entity,
            propertyName,
            propertyValue,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceAlreadyExists_typed()
    {
        // arrange
        const string entity = "Entity";
        const string propertyName = "Id";
        const string propertyValue = "1";

        var description =
            string.Format(SondorErrorMessages.ResourceAlreadyExists, entity, propertyName, propertyValue);
        var error = new SondorError(SondorErrorCodes.ResourceAlreadyExists,
            SondorErrorTypes.ResourceAlreadyExistsType,
            description);
        var expected = new SondorResult<int>(error);

        // act
        var result = await _resultManager.ResourceAlreadyExists<int>(entity,
            propertyName,
            propertyValue,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceCreateFailed{TResource}"/> throws exceptions as expected.
    /// </summary>
    /// <param name="resourceName">The resource name.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceCreateFailed_resource_exception(string? resourceName)
    {
        // arrange
        const int resource = 10;

        // exceptions
        if (resourceName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceCreateFailed(resourceName!, resource));

            return;
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceCreateFailed(resourceName, resource));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceCreateFailed(resourceName, resource));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceCreateFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceCreateFailed()
    {
        // arrange
        const int resource = 10;
        const string resourceName = "test";

        var description =
            string.Format(SondorErrorMessages.ResourceCreateFailed, resourceName);
        var error = new SondorError(SondorErrorCodes.ResourceCreateFailed,
            SondorErrorTypes.ResourceCreateFailedType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.ResourceCreateFailed(resourceName,
            resource,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceCreateFailed_typed()
    {
        // arrange
        const int resource = 10;
        const string resourceName = "test";

        var description =
            string.Format(SondorErrorMessages.ResourceCreateFailed, resourceName);
        var error = new SondorError(SondorErrorCodes.ResourceCreateFailed,
            SondorErrorTypes.ResourceCreateFailedType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.ResourceCreateFailed<int, int>(resourceName,
            resource,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceUpdateFailed{TResource}"/> throws exceptions as expected.
    /// </summary>
    /// <param name="resourceName">The resource name.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceUpdateFailed_resource_exception(string? resourceName)
    {
        // arrange
        const int resource = 10;

        // exceptions
        if (resourceName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceUpdateFailed(resourceName!, resource));

            return;
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceUpdateFailed(resourceName, resource));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceUpdateFailed(resourceName, resource));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceUpdateFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceUpdateFailed()
    {
        // arrange
        const int resource = 10;
        const string resourceName = "test";

        var description =
            string.Format(SondorErrorMessages.ResourceUpdateFailed, resourceName);
        var error = new SondorError(SondorErrorCodes.ResourceUpdateFailed,
            SondorErrorTypes.ResourceUpdateFailedType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.ResourceUpdateFailed(resourceName,
            resource,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceUpdateFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceUpdateFailed_typed()
    {
        // arrange
        const int resource = 10;
        const string resourceName = "test";

        var description =
            string.Format(SondorErrorMessages.ResourceUpdateFailed, resourceName);
        var error = new SondorError(SondorErrorCodes.ResourceUpdateFailed,
            SondorErrorTypes.ResourceUpdateFailedType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.ResourceUpdateFailed<int, int>(resourceName,
            resource,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceDeleteFailed{TResource}"/> throws exceptions as expected.
    /// </summary>
    /// <param name="resourceName">The resource name.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceDeleteFailed_resource_exception(string? resourceName)
    {
        // arrange

        // exceptions
        if (resourceName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceDeleteFailed(resourceName!));

            return;
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceDeleteFailed(resourceName));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceDeleteFailed(resourceName));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceDeleteFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceDeleteFailed()
    {
        // arrange
        const string resourceName = "test";

        var description =
            string.Format(SondorErrorMessages.ResourceDeleteFailed, resourceName);
        var error = new SondorError(SondorErrorCodes.ResourceDeleteFailed,
            SondorErrorTypes.ResourceDeleteFailedType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.ResourceDeleteFailed(resourceName,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceDeleteFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceDeleteFailed_typed()
    {
        // arrange
        const string resourceName = "test";

        var description =
            string.Format(SondorErrorMessages.ResourceDeleteFailed, resourceName);
        var error = new SondorError(SondorErrorCodes.ResourceDeleteFailed,
            SondorErrorTypes.ResourceDeleteFailedType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.ResourceDeleteFailed<int>(resourceName,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Success"/> works as intended.
    /// </summary>
    [Test]
    public void Success()
    {
        // arrange
        var expected = new SondorResult();

        // act
        var result = _resultManager.Success();

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Success{TResult}"/> works as intended.
    /// </summary>
    [Test]
    public void Success_typed()
    {
        // arrange
        const int value = 10;
        var expected = new SondorResult<int>(value);

        // act
        var result = _resultManager.Success(value);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError"/> throws exceptions as expected.
    /// </summary>
    /// <param name="message">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void UnexpectedError_entity_exception(string? message)
    {
        // exceptions
        if (message is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.UnexpectedError(message!));

            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.UnexpectedError(message));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.UnexpectedError(message));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError"/> throws exceptions as expected.
    /// </summary>
    /// <param name="message">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void UnexpectedError_typed_entity_exception(string? message)
    {
        // exceptions
        if (message is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.UnexpectedError<int>(message!));

            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.UnexpectedError<int>(message));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.UnexpectedError<int>(message));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError"/> works as intended.
    /// </summary>
    /// <param name="message">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public async Task UnexpectedError(string? message)
    {
        // exceptions
        if (message is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.UnexpectedError(message!));

            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.UnexpectedError(message));

            return;
        }

        // arrange
        var error = new SondorError(SondorErrorCodes.UnexpectedError, SondorErrorTypes.UnexpectedErrorType, message);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.UnexpectedError(message, TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError{TResult}"/> works as intended.
    /// </summary>
    /// <param name="message">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public async Task UnexpectedError_typed(string? message)
    {
        // exceptions
        if (message is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.UnexpectedError<int>(message!));

            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.UnexpectedError<int>(message));

            return;
        }

        // arrange
        var error = new SondorError(SondorErrorCodes.UnexpectedError, SondorErrorTypes.UnexpectedErrorType, message);
        var expected = new SondorResult<int>(error);

        // act
        var result = await _resultManager.UnexpectedError<int>(message, TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> throws exceptions as expected.
    /// </summary>
    /// <param name="entityName">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceNotFound_entity_exception(string? entityName)
    {
        // arrange
        const string propertyName = "Id";
        const string propertyValue = "1";

        // exceptions
        if (entityName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceNotFound(entityName!, propertyName, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> throws exceptions as expected.
    /// </summary>
    /// <param name="entityName">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceNotFound_typed_entity_exception(string? entityName)
    {
        // arrange
        const string propertyName = "Id";
        const string propertyValue = "1";

        // exceptions
        if (entityName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceNotFound<int>(entityName!, propertyName, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> throws exceptions as expected.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceNotFound_property_name_exception(string? propertyName)
    {
        // arrange
        const string entityName = "Entity";
        const string propertyValue = "1";

        // exceptions
        if (propertyName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceNotFound(entityName, propertyName!, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> throws exceptions as expected.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceNotFound_typed_property_name_exception(string? propertyName)
    {
        // arrange
        const string entityName = "Entity";
        const string propertyValue = "1";

        // exceptions
        if (propertyName is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName!, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> throws exceptions as expected.
    /// </summary>
    /// <param name="propertyValue">The property value.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceNotFound_property_value_exception(string? propertyValue)
    {
        // arrange
        const string entityName = "Entity";
        const string propertyName = "Id";

        // exceptions
        if (propertyValue is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue!));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> throws exceptions as expected.
    /// </summary>
    /// <param name="propertyValue">The property value.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void ResourceNotFound_typed_property_value_exception(string? propertyValue)
    {
        // arrange
        const string entityName = "Entity";
        const string propertyName = "Id";

        // exceptions
        if (propertyValue is null)
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue!));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            Assert.ThrowsAsync<ArgumentException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrowAsync(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceNotFound()
    {
        // arrange
        const string entity = "Entity";
        const string propertyName = "Id";
        const string propertyValue = "1";

        var description =
            string.Format(SondorErrorMessages.ResourceNotFound, entity, propertyName, propertyValue);
        var error = new SondorError(SondorErrorCodes.ResourceNotFound,
            SondorErrorTypes.ResourceNotFoundType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.ResourceNotFound(entity,
            propertyName,
            propertyValue,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> works as intended.
    /// </summary>
    [Test]
    public async Task ResourceNotFound_typed()
    {
        // arrange
        const string entity = "Entity";
        const string propertyName = "Id";
        const string propertyValue = "1";

        var description =
            string.Format(SondorErrorMessages.ResourceNotFound, entity, propertyName, propertyValue);
        var error = new SondorError(SondorErrorCodes.ResourceNotFound,
            SondorErrorTypes.ResourceNotFoundType,
            description);
        var expected = new SondorResult<int>(error);

        // act
        var result = await _resultManager.ResourceNotFound<int>(entity,
            propertyName,
            propertyValue,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError"/> works as intended.
    /// </summary>
    [Test]
    public async Task UnexpectedError()
    {
        // arrange
        const string message = "test-message";

        var description =
            string.Format(SondorErrorMessages.UnexpectedError, message);
        var error = new SondorError(SondorErrorCodes.UnexpectedError,
            SondorErrorTypes.UnexpectedErrorType,
            description);
        var expected = new SondorResult(error);

        // act
        var result = await _resultManager.UnexpectedError(description,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError"/> works as intended.
    /// </summary>
    [Test]
    public async Task UnexpectedError_typed()
    {
        // arrange
        const string message = "test-message";

        var description =
            string.Format(SondorErrorMessages.UnexpectedError, message);
        var error = new SondorError(SondorErrorCodes.UnexpectedError,
            SondorErrorTypes.UnexpectedErrorType,
            description);
        var expected = new SondorResult<int>(error);

        // act
        var result = await _resultManager.UnexpectedError<int>(description,
            TestContext.CurrentContext.CancellationToken);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }
}