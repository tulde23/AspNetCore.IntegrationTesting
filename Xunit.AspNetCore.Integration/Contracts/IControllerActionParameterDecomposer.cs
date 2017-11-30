namespace Xunit.AspNetCore.Integration.Contracts
{
    /// <summary>
    /// Describes an abstraction to produce the inverse of a model binding operation.  Basically we want the ability to take
    /// a controller action and decompose it's input parameters back into a valid URI.
    /// </summary>
    public interface IControllerActionParameterDecomposer
    {
        /// <summary>
        /// Resolves the specified controller action.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        void Decompose(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute);

        /// <summary>
        /// Determines whether this instance can decompose the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///   <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        bool CanDecompose(IControllerActionParameter parameter);
    }
}