using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sondor.Errors;
using Sondor.Errors.Tests;
using Sondor.ProblemResults.Constants;
using Sondor.ProblemResults.Extensions;
using Sondor.ResultManager.Extensions;
using Sondor.Tests.Args;
using Sondor.Translations.Args;
using Sondor.Translations.Extensions;
using Sondor.Translations.Options;
using System.Globalization;

namespace Sondor.ResultManager.Tests.Extensions;

/// <summary>
/// Tests for <see cref="SondorResultManagerExtensions"/>.
/// </summary>
[TestFixtureSource(typeof(LanguageArgs))]
public class SondorResultManagerExtensionsTests
{
    /// <summary>
    /// The language.
    /// </summary>
    private readonly string _language;

    /// <summary>
    /// The trace identifier.
    /// </summary>
    private const string TraceIdentifier = "trace-id";

    /// <summary>
    /// The result manager.
    /// </summary>
    private readonly ISondorResultManager _resultManager;

    /// <summary>
    /// Creates a new instance of <see cref="SondorResultManagerExtensionsTests"/>.
    /// </summary>
    public SondorResultManagerExtensionsTests(string language)
    {
        _language = language;
        var options = new SondorTranslationOptions
        {
            DefaultCulture = "en",
            SupportedCultures = new LanguageArgs().Cast<string>().ToArray(),
            UseKeyAsDefaultValue = false
        };
        var services = new ServiceCollection()
            .AddLogging()
            .AddTestTranslation(options, "Test:Translation")
            .AddSingleton<IHttpContextAccessor>(_ => new HttpContextAccessor
            {
                HttpContext = CreateHttpContext()
            })
            .AddSondorResultManager();

        var serviceProvider = services.BuildServiceProvider();
        _resultManager = serviceProvider.GetRequiredService<ISondorResultManager>();
    }

    /// <summary>
    /// Test setup.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        var culture = new CultureInfo(_language);

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.BadRequest"/> works as intended.
    /// </summary>
    [Test]
    public void BadRequest()
    {
        // arrange
        const string error = "test";

        var expected = FromErrorCode(SondorErrorCodes.BadRequest, CreateHttpContext());

        // act
        var result = _resultManager.BadRequest(error);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.BadRequest"/> works as intended.
    /// </summary>
    [Test]
    public void BadRequest_typed()
    {
        // arrange
        const string error = "test";

        var expected = FromErrorCode<int>(SondorErrorCodes.BadRequest, CreateHttpContext());

        // act
        var result = _resultManager.BadRequest<int>(error);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Validation"/> works as intended.
    /// </summary>
    [Test]
    public void Validation()
    {
        // arrange
        var failures = new List<ValidationFailure>
        {
            new ("Id", "Invalid id", 0)
        };

        var expected = FromErrorCode(SondorErrorCodes.ValidationFailed, CreateHttpContext());

        // act
        var result = _resultManager.Validation(failures);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Validation"/> works as intended.
    /// </summary>
    [Test]
    public void Validation_typed()
    {
        // arrange
        var failures = new List<ValidationFailure>
        {
            new ("Id", "Invalid id", 0)
        };

        var expected = FromErrorCode<int>(SondorErrorCodes.ValidationFailed, CreateHttpContext());

        // act
        var result = _resultManager.Validation<int>(failures);

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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists(entityName!, propertyName, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists<int>(entityName!, propertyName, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName!, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName!, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue!));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceAlreadyExists(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue!));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceAlreadyExists<int>(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceAlreadyExists()
    {
        // arrange
        const string entity = "test";
        const string propertyName = "property-name";
        const string propertyValue = "property-value";

        var expected = FromErrorCode(SondorErrorCodes.ResourceAlreadyExists, CreateHttpContext());

        // act
        var result = _resultManager.ResourceAlreadyExists(entity, propertyName, propertyValue);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceAlreadyExists_typed()
    {
        // arrange
        const string entity = "test";
        const string propertyName = "property-name";
        const string propertyValue = "property-value";

        var expected = FromErrorCode<int>(SondorErrorCodes.ResourceAlreadyExists, CreateHttpContext());

        // act
        var result = _resultManager.ResourceAlreadyExists<int>(entity, propertyName, propertyValue);

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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceCreateFailed(resourceName!, resource));

            return;
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceCreateFailed(resourceName, resource));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceCreateFailed(resourceName, resource));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceCreateFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceCreateFailed()
    {
        // arrange
        const int resource = 10;
        const string resourceName = "test";

        var expected = FromErrorCode(SondorErrorCodes.ResourceCreateFailed, CreateHttpContext());

        // act
        var result = _resultManager.ResourceCreateFailed(resourceName, resource);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceAlreadyExists"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceCreateFailed_typed()
    {
        // arrange
        const int resource = 10;
        const string resourceName = "test";

        var expected = FromErrorCode(SondorErrorCodes.ResourceCreateFailed, CreateHttpContext());

        // act
        var result = _resultManager.ResourceCreateFailed<int, int>(resourceName, resource);

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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceUpdateFailed(resourceName!, resource));

            return;
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceUpdateFailed(resourceName, resource));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceUpdateFailed(resourceName, resource));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceUpdateFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceUpdateFailed()
    {
        // arrange
        const int resource = 10;
        const string resourceName = "test";

        var expected = FromErrorCode(SondorErrorCodes.ResourceUpdateFailed, CreateHttpContext());

        // act
        var result = _resultManager.ResourceUpdateFailed(resourceName, resource);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceUpdateFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceUpdateFailed_typed()
    {
        // arrange
        const int resource = 10;
        const string resourceName = "test";

        var expected = FromErrorCode(SondorErrorCodes.ResourceUpdateFailed, CreateHttpContext());

        // act
        var result = _resultManager.ResourceUpdateFailed<int, int>(resourceName, resource);

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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceDeleteFailed(resourceName!));

            return;
        }

        if (string.IsNullOrWhiteSpace(resourceName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceDeleteFailed(resourceName));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceDeleteFailed(resourceName));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceDeleteFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceDeleteFailed()
    {
        // arrange
        const string resourceName = "test";

        var expected = FromErrorCode(SondorErrorCodes.ResourceDeleteFailed, CreateHttpContext());

        // act
        var result = _resultManager.ResourceDeleteFailed(resourceName);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceDeleteFailed{TResource}"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceDeleteFailed_typed()
    {
        // arrange
        const string resourceName = "test";

        var expected = FromErrorCode(SondorErrorCodes.ResourceDeleteFailed, CreateHttpContext());

        // act
        var result = _resultManager.ResourceDeleteFailed<int>(resourceName);

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
            Assert.Throws<ArgumentNullException>(() => _resultManager.UnexpectedError(message!));

            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.UnexpectedError(message));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.UnexpectedError(message));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.UnexpectedError<int>(message!));

            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.UnexpectedError<int>(message));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.UnexpectedError<int>(message));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError"/> works as intended.
    /// </summary>
    /// <param name="message">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void UnexpectedError(string? message)
    {
        // exceptions
        if (message is null)
        {
            Assert.Throws<ArgumentNullException>(() => _resultManager.UnexpectedError(message!));

            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.UnexpectedError(message));

            return;
        }

        // arrange
        var expected = FromErrorCode(SondorErrorCodes.UnexpectedError, CreateHttpContext());

        // act
        var result = _resultManager.UnexpectedError(message);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError{TResult}"/> works as intended.
    /// </summary>
    /// <param name="message">The message.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void UnexpectedError_typed(string? message)
    {
        // exceptions
        if (message is null)
        {
            Assert.Throws<ArgumentNullException>(() => _resultManager.UnexpectedError<int>(message!));

            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.UnexpectedError<int>(message));

            return;
        }

        // arrange
        var expected = FromErrorCode<int>(SondorErrorCodes.UnexpectedError, CreateHttpContext());

        // act
        var result = _resultManager.UnexpectedError<int>(message);

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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceNotFound(entityName!, propertyName, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceNotFound<int>(entityName!, propertyName, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceNotFound(entityName, propertyName!, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName!, propertyValue));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue!));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceNotFound(entityName, propertyName, propertyValue));
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
            Assert.Throws<ArgumentNullException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue!));

            return;
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.ResourceNotFound<int>(entityName, propertyName, propertyValue));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceNotFound()
    {
        // arrange
        const string entity = "test";
        const string propertyName = "property-name";
        const string propertyValue = "property-value";

        var expected = FromErrorCode(SondorErrorCodes.ResourceNotFound, CreateHttpContext());

        // act
        var result = _resultManager.ResourceNotFound(entity, propertyName, propertyValue);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.ResourceNotFound"/> works as intended.
    /// </summary>
    [Test]
    public void ResourceNotFound_typed()
    {
        // arrange
        const string entity = "test";
        const string propertyName = "property-name";
        const string propertyValue = "property-value";

        var expected = FromErrorCode<int>(SondorErrorCodes.ResourceNotFound, CreateHttpContext());

        // act
        var result = _resultManager.ResourceNotFound<int>(entity, propertyName, propertyValue);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError"/> works as intended.
    /// </summary>
    [Test]
    public void UnexpectedError()
    {
        // arrange
        const string message = "test-message";

        var expected = FromErrorCode(SondorErrorCodes.UnexpectedError, CreateHttpContext());

        // act
        var result = _resultManager.UnexpectedError(message);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.UnexpectedError"/> works as intended.
    /// </summary>
    [Test]
    public void UnexpectedError_typed()
    {
        // arrange
        const string message = "test-message";

        var expected = FromErrorCode<int>(SondorErrorCodes.UnexpectedError, CreateHttpContext());

        // act
        var result = _resultManager.UnexpectedError<int>(message);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Unauthorized"/> throws exceptions as expected.
    /// </summary>
    /// <param name="resource">The resource.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void Unauthorized_property_value_exception(string? resource)
    {
        // exceptions
        if (resource is null)
        {
            Assert.Throws<ArgumentNullException>(() => _resultManager.Unauthorized(resource!));

            return;
        }

        if (string.IsNullOrWhiteSpace(resource))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.Unauthorized(resource));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.Unauthorized(resource));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Unauthorized"/> throws exceptions as expected.
    /// </summary>
    /// <param name="resource">The resource.</param>
    [TestCaseSource(typeof(StringArgs))]
    public void Unauthorized_typed_property_value_exception(string? resource)
    {
        // exceptions
        if (resource is null)
        {
            Assert.Throws<ArgumentNullException>(() => _resultManager.Unauthorized<int>(resource!));

            return;
        }

        if (string.IsNullOrWhiteSpace(resource))
        {
            Assert.Throws<ArgumentException>(() => _resultManager.Unauthorized<int>(resource));

            return;
        }

        Assert.DoesNotThrow(() => _resultManager.Unauthorized<int>(resource));
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Unauthorized"/> works as intended.
    /// </summary>
    [Test]
    public void Unauthorized()
    {
        // arrange
        const string resource = "test";

        var expected = FromErrorCode(SondorErrorCodes.Unauthorized, CreateHttpContext());

        // act
        var result = _resultManager.Unauthorized(resource);

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Unauthorized"/> works as intended.
    /// </summary>
    [Test]
    public void Unauthorized_typed()
    {
        // arrange
        const string resource = "test";

        var expected = FromErrorCode<int>(SondorErrorCodes.Unauthorized, CreateHttpContext());

        // act
        var result = _resultManager.Unauthorized<int>(resource);

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Forbidden"/> works as intended.
    /// </summary>
    [Test]
    public void Forbidden()
    {
        // arrange
        var expected = FromErrorCode(SondorErrorCodes.Forbidden, CreateHttpContext());

        // act
        var result = _resultManager.Forbidden();

        //assert
        SondorErrorAssert.AssertResult(result, expected);
    }

    /// <summary>
    /// Ensures that <see cref="SondorResultManagerExtensions.Forbidden"/> works as intended.
    /// </summary>
    [Test]
    public void Forbidden_typed()
    {
        // arrange
        var expected = FromErrorCode<int>(SondorErrorCodes.Forbidden, CreateHttpContext());

        // act
        var result = _resultManager.Forbidden<int>();

        //assert
        SondorErrorAssert.AssertResult<int>(result, expected);
    }

    /// <summary>
    /// Create default HTTP context.
    /// </summary>
    /// <returns>Returns the HTTP context.</returns>
    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext
        {
            Request =
            {
                Method = HttpMethod.Get.Method,
                Path = "/test",
                Host = new HostString("localhost"),
                Protocol = "HTTP/1.1"
            },
            TraceIdentifier = TraceIdentifier
        };

        return context;
    }

    /// <summary>
    /// Create a <see cref="SondorResult"/> from the provided <paramref name="errorCode"/>.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Returns the result.</returns>
    private SondorResult FromErrorCode(int errorCode,
        HttpContext context)
    {
        const string resource = "test";
        const string newResource = "new-resource";
        const string propertyName = "property-name";
        const string propertyValue = "property-value";
        const string updatedResource = "updated-resource";

        ValidationFailure[] validationErrors = [new("test", "error")];
        var patches = new Dictionary<string, string?>([new KeyValuePair<string, string?>("patch-1", "value")]);
        string[] reasons = ["reason-1"];

        return errorCode switch
        {
            SondorErrorCodes.BadRequest => new SondorResult(new SondorError(SondorErrorCodes.BadRequest,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                resource,
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.ErrorMessage, resource }
                })),
            SondorErrorCodes.Forbidden => new SondorResult(new SondorError(SondorErrorCodes.Forbidden,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemForbidden(),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemForbidden() },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.Forbidden },
                    { ProblemResultConstants.Resource, context.GetInstance() }
                })),
            SondorErrorCodes.ResourceAlreadyExists => new SondorResult(new SondorError(SondorErrorCodes.ResourceAlreadyExists,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemResourceAlreadyExists(resource, propertyName, propertyValue),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemResourceAlreadyExists(resource, propertyName, propertyValue) },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.PropertyName, propertyName },
                    { ProblemResultConstants.PropertyValue, propertyValue }
                })),
            SondorErrorCodes.ResourceCreateFailed => new SondorResult(new SondorError(SondorErrorCodes.ResourceCreateFailed,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemResourceCreateFailed(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemResourceCreateFailed(resource) },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.NewResource, newResource }
                })),
            SondorErrorCodes.ResourceDeleteFailed => new SondorResult(new SondorError(SondorErrorCodes.ResourceDeleteFailed,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemResourceDeleteFailed(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceDeleteFailed },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemResourceDeleteFailed(resource) },
                    { ProblemResultConstants.Reasons, reasons },
                    { ProblemResultConstants.Resource, resource }
                })),
            SondorErrorCodes.ResourceNotFound => new SondorResult(new SondorError(SondorErrorCodes.ResourceNotFound,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemResourceNotFound(resource, propertyName, propertyValue),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemResourceNotFound(resource, propertyName, propertyValue) },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.PropertyName, propertyName },
                    { ProblemResultConstants.PropertyValue, propertyValue }
                })),
            SondorErrorCodes.ResourcePatchFailed => new SondorResult(new SondorError(errorCode,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemResourcePatchFailed(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.Patches, patches },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourcePatchFailed },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemResourcePatchFailed(resource) },
                    { ProblemResultConstants.Resource, resource }
                })),
            SondorErrorCodes.ResourceUpdateFailed => new SondorResult(new SondorError(SondorErrorCodes.ResourceUpdateFailed,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemResourceUpdateFailed(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ResourceUpdateFailed },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemResourceUpdateFailed(resource) },
                    { ProblemResultConstants.Reasons, reasons },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.UpdatedResource, updatedResource }
                })),
            SondorErrorCodes.TaskCancelled => new SondorResult(new SondorError(SondorErrorCodes.TaskCancelled,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemTaskCancelled(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.TaskCancelled },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemTaskCancelled(resource) }
                })),
            SondorErrorCodes.ValidationFailed => new SondorResult(new SondorError(SondorErrorCodes.ValidationFailed,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemValidationErrors(1),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.ValidationFailed },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemValidationErrors(1) },
                    { ProblemResultConstants.Errors, validationErrors }
                })),
            SondorErrorCodes.Unauthorized => new SondorResult(new SondorError(SondorErrorCodes.Unauthorized,
                ProblemResultConstants.FindProblemTypeByErrorCode(errorCode),
                _resultManager.TranslationManager.ProblemUnauthorized(resource),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, errorCode },
                    { ProblemResultConstants.Resource, resource },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemUnauthorized(resource) }
                })),
            _ => new SondorResult(new SondorError(SondorErrorCodes.UnexpectedError,
                SondorErrorTypes.UnexpectedErrorType,
                _resultManager.TranslationManager.ProblemUnexpectedError(),
                new Dictionary<string, object?>
                {
                    { ProblemResultConstants.TraceKey, context.TraceIdentifier },
                    { ProblemResultConstants.ErrorCode, SondorErrorCodes.UnexpectedError },
                    { ProblemResultConstants.ErrorMessage, _resultManager.TranslationManager.ProblemUnexpectedError() }
                })),
        };
    }

    /// <inheritdoc cref="FromErrorCode"/>
    /// <typeparam name="TResult">The result type.</typeparam>
    private SondorResult<TResult> FromErrorCode<TResult>(int errorCode,
        HttpContext context)
    {
        var result = FromErrorCode(errorCode, context);
     
        return new SondorResult<TResult>(result.Error!);
    }
}