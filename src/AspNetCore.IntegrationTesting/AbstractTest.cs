using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCore.IntegrationTesting
{
    /// <summary>
    /// A common base class for all tests that require a fixture
    /// </summary>
    /// <typeparam name="TFixture">A fixture for this test </typeparam>
    public abstract class AbstractTest<TEntryPoint> : IClassFixture<TEntryPoint>, IDisposable where TEntryPoint : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTest{TFixture}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected AbstractTest(WebApplicationFactory<TEntryPoint> factory)
        {
            Factory = factory;

            var type = Logger.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            CurrentTest = (ITest)testMember?.GetValue(Logger);
            Client = Factory.CreateClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTest{TFixture}" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="testOutputHelper">The test output helper.  This provides Xunit logging</param>
        protected AbstractTest(WebApplicationFactory<TEntryPoint> factory, ITestOutputHelper testOutputHelper) : this(factory)
        {
            Logger = testOutputHelper;
        }

        protected HttpClient Client { get; }

        /// <summary>
        /// Gets  the web application factory instance
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>M S
        public WebApplicationFactory<TEntryPoint> Factory { get; }

        /// <summary>
        /// Gets the current test.
        /// </summary>
        /// <value>
        /// The current test.
        /// </value>
        protected ITest CurrentTest { get; }

        /// <summary>
        /// Gets the xunit logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ITestOutputHelper Logger { get; }

        /// <summary>
        /// Creates the HTTP request message from a lambda expression
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        protected HttpRequestMessage CreateHttpRequestMessage<TController, TResponse>(
             Expression<Func<TController, TResponse>> expression) where TController : ControllerBase
        {
            if (expression == null)
            {
                throw new System.ArgumentNullException(nameof(expression));
            }
            var controllerAction = ControllerActionFactory.GetAction(expression);
            var route = ControllerActionRouteFactory.CreateRoute(controllerAction);
            return route.BuildRequestMessage(controllerAction);
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}