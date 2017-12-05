using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;


namespace AspNetCore.IntegrationTesting.Contracts
{
    /// <summary>
    /// Describes an invokable route.
    /// </summary>
    public interface IControllerActionRoute
    {
        /// <summary>
        /// Adds to a new segment to the templated route.
        /// </summary>
        /// <param name="routeSegment">The route segment.</param>
        void AddToRoute(string routeSegment);

    
        /// <summary>
        /// Sets a model.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetModel(object value);
        /// <summary>
        /// Sets a query string parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void SetQueryStringParameter(string name, string value);

        /// <summary>
        /// Sets a route value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void SetRouteValue(string name, object value);

        /// <summary>
        /// Sets a header value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void SetHeaderValue(string name, object value);

        /// <summary>
        /// Builds the http request message
        /// </summary>
        /// <returns></returns>
        HttpRequestMessage BuildRequestMessage(IControllerAction controllerAction);
    }
}
