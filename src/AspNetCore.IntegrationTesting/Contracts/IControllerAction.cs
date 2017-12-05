using System;
using System.Collections.Generic;
using System.Net.Http;

namespace AspNetCore.IntegrationTesting.Contracts
{
    /// <summary>
    /// Describers a controller action and parameters
    /// </summary>
    public interface IControllerAction
    {
        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
        string Controller { get; }
        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        string ActionName { get; }
        /// <summary>
        /// Gets the controller action return type
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
        Type ReturnType { get; }
        /// <summary>
        /// Gets the HttpMethod of the controller action
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        HttpMethod Method { get; }
        /// <summary>
        /// Gets a list of all controller action input parameters
        /// </summary>
        /// <value>
        /// The action parameters.
        /// </value>
        List<IControllerActionParameter> ActionParameters { get; }
        /// <summary>
        /// Contains a list of route segment string extracted from both controller route annotations and controller action annotations.
        /// </summary>
        /// <value>
        /// The route segments.
        /// </value>
        List<string> RouteSegments { get; }
    }
}