using Xunit.AspNetCore.Integration.Contracts;

namespace Xunit.AspNetCore.Integration.Decomposing
{
    /// <summary>
    ///  A factory class for creating IControllerActionRoute instances
    /// </summary>
    public static class ControllerActionRouteFactory
    {
        /// <summary>
        /// The provider
        /// </summary>
        private static readonly IControllerActionRouteProvider _provider = new ControllerActionRouteProvider();

        /// <summary>
        /// Creates a route from a controller action
        /// </summary>
        /// <param name="controllerAction">The controller action.</param>
        /// <returns></returns>
        public static IControllerActionRoute CreateRoute(IControllerAction controllerAction)
        {
            return _provider.CreateRoute(controllerAction);
        }
    }
}