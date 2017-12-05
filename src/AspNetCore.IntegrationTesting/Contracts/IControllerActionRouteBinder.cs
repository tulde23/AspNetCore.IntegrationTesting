namespace AspNetCore.IntegrationTesting.Contracts
{
    /// <summary>
    /// Describes an abstraction to produce the inverse of a model binding operation.  Basically we want the ability to take
    /// a controller action and decompose it's input parameters back into a valid URI.
    /// </summary>
    public interface IControllerActionRouteBinder
    {
        /// <summary>
        /// Binds the parameter to a route
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        void Bind(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute);

        /// <summary>
        /// Determines whether this instance can bind the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///   <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        bool CanBind(IControllerActionParameter parameter);
    }
}