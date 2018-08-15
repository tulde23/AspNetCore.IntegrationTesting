using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
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
            return RouteHelper.BuildRequestMessage(expression, TestOutputHelper);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Client?.Dispose();
        }

        /// <summary>
        /// Invokes an IControllerAction
        /// </summary>
        /// <typeparam name="TController">The type of the ControllerBase.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">controllerAction</exception>
        protected async Task<TResponse> InvokeAsync<TController, TResponse>(
            Expression<Func<TController, TResponse>> expression) where TController : ControllerBase
        {
            var message = RouteHelper.BuildRequestMessage(expression, TestOutputHelper);
            var response = await Client.SendAsync(message);
            var dataAsString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(dataAsString);
        }

        /// <summary>
        /// Invokes an IControllerAction that returns a generic Task.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        protected TResponse InvokeAsyncTask<TController, TResponse>(Expression<Func<TController, TResponse>> expression) where TResponse : Task where TController : ControllerBase
        {
            if (expression == null)
            {
                throw new System.ArgumentNullException(nameof(expression));
            }
            var responseType = typeof(TResponse);
            //if generic, get the actual type
            if (responseType.IsGenericType)
            {
                responseType = responseType.GetGenericArguments()[0];
            }
            var response = DispatchRequest(Client, expression).GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject(response, responseType);
            var method = typeof(Task).GetMethod("FromResult");
            var genericMethod = method.MakeGenericMethod(responseType);
            return (TResponse)genericMethod.Invoke(null, new object[] { result });
        }

        /// <summary>
        /// Dispatches the request.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        private async Task<string> DispatchRequest<TController, TResponse>(HttpClient client, Expression<Func<TController, TResponse>> expression) where TController : ControllerBase
        {
            var message = RouteHelper.BuildRequestMessage(expression, TestOutputHelper);
            var response = await client.SendAsync(message);
            var dataAsString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return dataAsString;
        }
    }
}