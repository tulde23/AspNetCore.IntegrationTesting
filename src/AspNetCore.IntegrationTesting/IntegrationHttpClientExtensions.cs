using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.IntegrationTesting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspNetCore
{
    /// <summary>
    ///
    /// </summary>
    public static class IntegrationHttpClientExtensions
    {
        /// <summary>
        /// Invokes an IControllerAction
        /// </summary>
        /// <typeparam name="TController">The type of the ControllerBase.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">controllerAction</exception>
        public static async Task<TResponse> InvokeAsync<TController, TResponse>(this HttpClient client,
            Expression<Func<TController, TResponse>> expression) where TController : ControllerBase
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

        /// <summary>
        /// Invokes an IControllerAction that returns a generic Task.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static  TResponse InvokeAsyncTask<TController, TResponse>(this HttpClient client, Expression<Func<TController, TResponse>> expression) where TResponse : Task
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
            var response = DispatchRequest(client, expression).GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject(response, responseType);
            var method = typeof(Task).GetMethod("FromResult");
            var genericMethod = method.MakeGenericMethod(responseType);
            return (TResponse)genericMethod.Invoke(null, new object[] { result });
        }

        private static async Task<string> DispatchRequest<TController, TResponse>(HttpClient client, Expression<Func<TController, TResponse>> expression)
        {
            var controllerAction = ControllerActionFactory.GetAction(expression);
            var route = ControllerActionRouteFactory.CreateRoute(controllerAction);
            var response = await client.SendAsync(route.BuildRequestMessage(controllerAction));
            var dataAsString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            return dataAsString;
        }
    }
}