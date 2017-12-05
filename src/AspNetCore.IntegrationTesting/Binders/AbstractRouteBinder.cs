using AspNetCore.IntegrationTesting.Contracts;

namespace AspNetCore.IntegrationTesting.Binders
{
    /// <summary>
    /// An abstract base class for all action route binders.
    /// </summary>
    /// <seealso cref="IControllerActionRouteBinder" />
    public abstract class AbstractRouteBinder : IControllerActionRouteBinder
    {

        /// <summary>
        /// Determines whether this instance can bind the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool CanBind(IControllerActionParameter parameter);


        /// <summary>
        /// Binds the parameter to a route
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        public void Bind(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {
            //ignore parameters with null values.
            if( parameter.ParameterValue == null)
            {
                return;
            }
            BindParameter(parameter, controllerActionRoute);
        }

        /// <summary>
        /// Binds the parameter to a route.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        protected abstract void BindParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute);

        /// <summary>
        /// Determines whether BindingSourceMetadata is of the specified type.
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