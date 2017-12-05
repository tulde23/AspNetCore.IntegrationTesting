using System.Linq;
using System.Web;
using AspNetCore.IntegrationTesting.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.IntegrationTesting.Binders
{
    /// <summary>
    /// Annotated by the [FromQuery] attribute, assumes the parameter will be sent as a query string parameter on the request
    /// </summary>
    /// <seealso cref="AbstractRouteBinder" />
    internal class FromQueryBinder : AbstractRouteBinder
    {
        /// <summary>
        /// Binds the parameter to a route.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        protected override void BindParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {
            if (parameter.ParameterValue is string || parameter.ParameterValue.GetType().IsPrimitive)
            {
                controllerActionRoute.SetQueryStringParameter(parameter.ParameterName, parameter.ParameterValue.ToString());
            }
            else
            {
                //assume we need to pass the model as a query string parameter
                var properties = from p in parameter.ParameterValue.GetType().GetProperties()
                                 where p.GetValue(parameter.ParameterValue, null) != null
                                 select new
                                 {
                                     Name = p.Name,
                                     Val = HttpUtility.UrlEncode(p.GetValue(parameter.ParameterValue, null).ToString())
                                 };

                foreach (var property in properties)
                {
                    controllerActionRoute.SetQueryStringParameter(property.Name, property.Val);
                }
            }
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
            return IsBindingSourceOfType<FromQueryAttribute>(parameter);
        }
    }
}