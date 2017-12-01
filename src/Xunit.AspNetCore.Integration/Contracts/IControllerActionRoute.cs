using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;


namespace Xunit.AspNetCore.Integration.Contracts
{
    /// <summary>
    /// Describes a route 
    /// </summary>
    public interface IControllerActionRoute
    {
        /// <summary>
        /// Adds to a new segment to the templated route.
        /// </summary>
        /// <param name="routeSegment">The route segment.</param>
        void AddToRoute(string routeSegment);

    
        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetModel(object value);
        /// <summary>
        /// Sets the query string parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void SetQueryStringParameter(string name, string value);

        /// <summary>
        /// Sets the route value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void SetRouteValue(string name, object value);

        /// <summary>
        /// Sets the header value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void SetHeaderValue(string name, object value);

        /// <summary>
        /// Builds the request message.
        /// </summary>
        /// <returns></returns>
        HttpRequestMessage BuildRequestMessage(IControllerAction controllerAction);
    }
}
