using AspNetCore.IntegrationTesting.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.IntegrationTesting.Binders
{
    /// <summary>
    /// Annotated by the [FromForm] attribute, assumes the parameter will be sent as the body or content of the request
    /// </summary>
    /// <seealso cref="AbstractRouteBinder" />
    internal class FromFormBinder : FromBodyBinder
    {
        /// <summary>
        /// Determines whether this instance can bind the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanBind(IControllerActionParameter parameter)
        {
            return IsBindingSourceOfType<FromFormAttribute>(parameter);
        }
    }
}