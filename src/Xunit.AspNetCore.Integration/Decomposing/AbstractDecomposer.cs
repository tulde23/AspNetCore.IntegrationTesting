using Xunit.AspNetCore.Integration.Contracts;

namespace Xunit.AspNetCore.Integration.Decomposing
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Xunit.AspNetCore.Integration.Contracts.IControllerActionParameterDecomposer" />
    public abstract class AbstractDecomposer : IControllerActionParameterDecomposer
    {
        /// <summary>
        /// Determines whether this instance can decompose the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool CanDecompose(IControllerActionParameter parameter);

        /// <summary>
        /// Resolves the specified controller action.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="controllerActionRoute"></param>
        public void Decompose(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {
            //ignore parameters with null values.
            if( parameter.ParameterValue == null)
            {
                return;
            }
            DecomposeParameter(parameter, controllerActionRoute);
        }

        /// <summary>
        /// Decomposes the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        protected abstract void DecomposeParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute);

        /// <summary>
        /// Determines whether [is binding source of type] [the specified paramter].
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute type.</typeparam>
        /// <param name="paramter">The paramter.</param>
        /// <returns>
        ///   <c>true</c> if [is binding source of type] [the specified paramter]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsBindingSourceOfType<TAttributeType>(IControllerActionParameter paramter)
        {
            return paramter.BindingSourceMetadata?.GetType().Equals(typeof(TAttributeType)) ?? false;
        }
    }
}