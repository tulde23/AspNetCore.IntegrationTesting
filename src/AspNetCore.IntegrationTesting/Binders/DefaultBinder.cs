using AspNetCore.IntegrationTesting.Contracts;

namespace AspNetCore.IntegrationTesting.Binders
{
    /// <summary>
    /// In the abscense of any model binding attributes, this will kick in by assuming the parameter is in the route
    /// </summary>
    internal class DefaultBinder : FromRouteBinder
    {
        /// <summary>
        /// Determines whether this instance can bind the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///   <c>true</c> if this instance can bind the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanBind(IControllerActionParameter parameter)
        {
            return parameter.BindingSourceMetadata == null;
        }
    }
}