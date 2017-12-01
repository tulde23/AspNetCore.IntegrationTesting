using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Xunit.AspNetCore.Integration.Contracts
{
    /// <summary>
    ///  Describes a controller action parameter and it's actual value
    /// </summary>
    public interface IControllerActionParameter
    {
        /// <summary>
        /// Gets the action( method name)
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        IControllerAction Action { get; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        string ParameterName { get; }

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <value>
        /// The parameter value.
        /// </value>
        object ParameterValue { get; }

        /// <summary>
        /// Gets the binding source metadata.
        /// </summary>
        /// <value>
        /// The binding source metadata.
        /// </value>
        IBindingSourceMetadata BindingSourceMetadata { get; }
    }
}