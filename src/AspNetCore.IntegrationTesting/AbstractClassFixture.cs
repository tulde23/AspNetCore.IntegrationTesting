using System;
using System.Linq.Expressions;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCore.IntegrationTesting
{
    /// <summary>
    /// An abstract base class for all integration tests.  Sets your test up a a class fixture which shared state amongst the tests in your class but no across multiple test classes.
    /// </summary>
    /// <typeparam name="TFixture">The type of the fixture.</typeparam>
    /// <typeparam name="TEntryPoint">The type of the entry point.</typeparam>
    /// <seealso cref="Xunit.IClassFixture{TFixture}" />
    /// <seealso cref="System.IDisposable" />
    public class AbstractClassFixture<TFixture, TEntryPoint> : IClassFixture<TFixture>, IDisposable where TEntryPoint : class where TFixture : WebApplicationFactory<TEntryPoint>
    {
        /// <summary>
        /// An instance of WEbApplicationFactory T
        /// </summary>
        /// <value>
        /// The factory.
        /// </value>
        public TFixture Factory { get; }

        /// <summary>
        /// Gets the HttpClient.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        protected HttpClient Client { get; }

        /// <summary>
        /// Gets the test output helper.
        /// </summary>
        /// <value>
        /// The test output helper.
        /// </value>
        public ITestOutputHelper TestOutputHelper { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractClassFixture{TFixture, TEntryPoint}"/> class.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <param name="testOutputHelper">The test output helper.</param>
        public AbstractClassFixture(TFixture fixture, ITestOutputHelper testOutputHelper)
        {
            Factory = fixture;
            Client = Factory.CreateClient();
            TestOutputHelper = testOutputHelper;
        }

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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}