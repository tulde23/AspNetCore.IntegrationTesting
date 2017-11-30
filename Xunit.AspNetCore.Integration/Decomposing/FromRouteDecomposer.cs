using Xunit.AspNetCore.Integration.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Xunit.AspNetCore.Integration.Decomposing
{
    /// <summary>
    /// Sets route values on the target route
    /// </summary>
    /// <seealso cref="Xunit.AspNetCore.Integration.Decomposing.AbstractDecomposer" />
    internal class FromRouteDecomposer : AbstractDecomposer
    {
        /// <summary>
        /// Decomposes the parameter.  Basically we want to match parameters passed to the action with tokens in the route string
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        protected override void DecomposeParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {
            controllerActionRoute.SetRouteValue(parameter.ParameterName, parameter.ParameterValue);
        }

        /// <summary>
        /// Determines whether this instance can decompose the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanDecompose(IControllerActionParameter parameter)
        {
            return IsBindingSourceOfType<FromRouteAttribute>(parameter);
        }
    }
}