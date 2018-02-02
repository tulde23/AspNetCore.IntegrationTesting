using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNetCore.IntegrationTesting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace System.Net.Http
{
    /// <summary>
    ///
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Translates a controller action into an http request using the InProc TestServer.  
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<TResponse>> ManyAsync<TController, TResponse>(this HttpClient client, Expression<Func<TController, object>> expression) where TController : Controller
        {
            return await client.InvokeAsync<TController, IEnumerable<TResponse>>(expression);
        }

        /// <summary>
        /// Translates a controller action into an http request using the InProc TestServer.  
        /// of items
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static async Task<TResponse> SingleAsync<TController, TResponse>(this HttpClient client, Expression<Func<TController, object>> expression) where TController : Controller
        {
            return await client.InvokeAsync<TController, TResponse>(expression);
        }

        /// <summary>
        /// Invokes an IControllerAction
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">controllerAction</exception>
        public static async Task<TResponse> InvokeAsync<TController, TResponse>(this HttpClient client,
            Expression<Func<TController, object>> expression) where TController : Controller
        {
            if (expression == null)
            {
                throw new System.ArgumentNullException(nameof(expression));
            }
            var controllerAction = ControllerActionFactory.GetAction(expression);
            var route = ControllerActionRouteFactory.CreateRoute(controllerAction);
            var response = await client.SendAsync(route.BuildRequestMessage(controllerAction));
            var dataAsString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(dataAsString);
        }
    }
}