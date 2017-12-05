using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AspNetCore.IntegrationTesting.Contracts;
using AspNetCore.IntegrationTesting.Decomposing;

namespace AspNetCore.IntegrationTesting
{
    /// <summary>
    /// Decorate the HttpClient with additional methods
    /// </summary>
    /// <seealso cref="AspNetCore.IntegrationTesting.Contracts.IControllerActionInvoker" />
    internal class ControllerActionHttpClientDecorator : IControllerActionInvoker
    {
        /// <summary>
        /// The client
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerActionHttpClientDecorator"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public ControllerActionHttpClientDecorator(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
        }

        /// <summary>
        /// Invokes an IControllerAction using an HttpClient
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="controllerAction">The controller action.</param>
        /// <returns></returns>
        public async Task<TResponse> InvokeAsync<TResponse>(IControllerAction controllerAction)
        {
            if (controllerAction == null)
            {
                throw new System.ArgumentNullException(nameof(controllerAction));
            }
            var route = ControllerActionRouteFactory.CreateRoute(controllerAction);
            var response = await _client.SendAsync(route.BuildRequestMessage(controllerAction));
            var dataAsString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(dataAsString);
        }
    }
}