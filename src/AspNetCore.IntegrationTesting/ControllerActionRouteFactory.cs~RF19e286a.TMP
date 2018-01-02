using AspNetCore.IntegrationTesting.Contracts;
using AspNetCore.IntegrationTesting.Models;

namespace AspNetCore.IntegrationTesting.Decomposing
{
    /// <summary>
    ///  A factory class for creating IControllerActionRoute instances
    /// </summary>
    public static class ControllerActionRouteFactory
    {
      
        /// <summary>
        /// Creates a route from a controller action
        /// </summary>
        /// <param name="controllerAction">The controller action.</param>
        /// <returns></returns>
        public static IControllerActionRoute CreateRoute(IControllerAction controllerAction)
        {
            var actionRoute = new ControllerActionRoute(controllerAction);
            foreach (var parameter in controllerAction.ActionParameters)
            {
                ControllerActionParameterBinders.Bind(parameter, actionRoute);
            }
            return actionRoute;
        }
    }
}