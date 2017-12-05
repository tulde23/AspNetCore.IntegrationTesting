using AspNetCore.IntegrationTesting.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.IntegrationTesting.Binders
{
    /// <summary>
    /// Annotated by the [FromHeader] attribute, assumes the parameter will be sent within the header of the request
    /// </summary>
    /// <seealso cref="AbstractRouteBinder" />
    internal class FromHeaderBinder : AbstractRouteBinder
    {

        /// <summary>
        /// Binds the parameter to a route.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        protected override void BindParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {
            controllerActionRoute.SetHeaderValue(parameter.ParameterName, parameter.ParameterValue);
        }


        /// <summary>
        /// Determines whether this instance can bind the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanBind(IControllerActionParameter parameter)
        {
            return IsBindingSourceOfType<FromHeaderAttribute>(parameter);
        }
    }
}