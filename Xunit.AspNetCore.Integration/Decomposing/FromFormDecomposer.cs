using Xunit.AspNetCore.Integration.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Xunit.AspNetCore.Integration.Decomposing
{
    /// <summary>
    /// Sets the model on the target route
    /// </summary>
    /// <seealso cref="Xunit.AspNetCore.Integration.Decomposing.FromBodyDecomposer" />
    internal class FromFormDecomposer : FromBodyDecomposer
    {
        /// <summary>
        /// Determines whether this instance can decompose the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanDecompose(IControllerActionParameter parameter)
        {
            return IsBindingSourceOfType<FromFormAttribute>(parameter);
        }
    }
}