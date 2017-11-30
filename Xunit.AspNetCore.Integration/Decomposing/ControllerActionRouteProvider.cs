using Xunit.AspNetCore.Integration.Contracts;

namespace Xunit.AspNetCore.Integration.Decomposing
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Xunit.AspNetCore.Integration.Contracts.IControllerActionRouteProvider" />
    public class ControllerActionRouteProvider : IControllerActionRouteProvider
    {
        /// <summary>
        /// Creates an IControllerActionRoute from an IControllerAction
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public IControllerActionRoute CreateRoute(IControllerAction action)
        {
            var actionRoute = new ControllerActionRoute(action);
            foreach (var parameter in action.ActionParameters)
            {
                ControllerActionParameterDecomposers.Decompose(parameter, actionRoute);
            }
            return actionRoute;
        }
    }
}