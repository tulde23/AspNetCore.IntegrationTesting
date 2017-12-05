using AspNetCore.IntegrationTesting.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.IntegrationTesting.Binders
{
    /// <summary>
    /// Annotated by the [FromRoute] attribute, assumes the parameter will be sent as a route parameter on the request.
    /// </summary>
    /// <seealso cref="AspNetCore.IntegrationTesting.Binders.AbstractRouteBinder" />
    /// <seealso cref="AbstractRouteBinder" />
    internal class FromRouteBinder : AbstractRouteBinder
    {

        /// <summary>
        /// Binds the parameter to a route.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        protected override void BindParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {
            controllerActionRoute.SetRouteValue(parameter.ParameterName, parameter.ParameterValue);
        }


        public override bool CanBind(IControllerActionParameter parameter)
        {
            return IsBindingSourceOfType<FromRouteAttribute>(parameter);
        }
    }
}