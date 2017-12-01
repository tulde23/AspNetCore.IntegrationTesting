using System.Linq;
using System.Web;
using Xunit.AspNetCore.Integration.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Xunit.AspNetCore.Integration.Decomposing
{
    /// <summary>
    /// Sets query string values on the target route
    /// </summary>
    /// <seealso cref="Xunit.AspNetCore.Integration.Decomposing.AbstractDecomposer" />
    internal class FromQueryDecomposer : AbstractDecomposer
    {
        /// <summary>
        /// Decomposes the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        protected override void DecomposeParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
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
        /// Determines whether this instance can decompose the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanDecompose(IControllerActionParameter parameter)
        {
            return IsBindingSourceOfType<FromQueryAttribute>(parameter);
        }
    }
}