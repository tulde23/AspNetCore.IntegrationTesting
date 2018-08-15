using System;
using System.Linq.Expressions;
using System.Net.Http;
using AspNetCore.IntegrationTesting.Contracts;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace AspNetCore.IntegrationTesting
{
    /// <summary>
    /// A utility class for interacting with routes
    /// </summary>
    public static class RouteHelper
    {
        /// <summary>
        /// Builds the request message
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="testOutputHelper">The test output helper for logging messages.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static HttpRequestMessage BuildRequestMessage<TController, TResponse>(
          Expression<Func<TController, TResponse>> expression, ITestOutputHelper testOutputHelper = null) where TController : ControllerBase
        {
            if (expression == null)
            {
                throw new System.ArgumentNullException(nameof(expression));
            }
            var controllerAction = ControllerActionFactory.GetAction(expression);
            var route = ControllerActionRouteFactory.CreateRoute(controllerAction);
            testOutputHelper?.WriteLine($"Built Route: {route?.ToString()} from expression");
            return route.BuildRequestMessage(controllerAction);
        }

        /// <summary>
        /// Creates a route instance
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static IControllerActionRoute GetRoute<TController, TResponse>(
          Expression<Func<TController, TResponse>> expression, ITestOutputHelper testOutputHelper = null) where TController : ControllerBase
        {
            if (expression == null)
            {
                throw new System.ArgumentNullException(nameof(expression));
            }
            var controllerAction = ControllerActionFactory.GetAction(expression);
            var route = ControllerActionRouteFactory.CreateRoute(controllerAction);
            testOutputHelper?.WriteLine($"Built Route: {route?.ToString()} from expression");
            return route;
        }
    }
}