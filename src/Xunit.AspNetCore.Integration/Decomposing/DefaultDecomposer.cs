using Xunit.AspNetCore.Integration.Contracts;

namespace Xunit.AspNetCore.Integration.Decomposing
{
    /// <summary>
    /// In the abscense of any model binding attributes, this will kick in by assuming the parameter is in the route
    /// </summary>
    /// <seealso cref="Xunit.AspNetCore.Integration.Contracts.IControllerActionParameterDecomposer" />
    internal class DefaultDecomposer : FromRouteDecomposer
    {
        public override bool CanDecompose(IControllerActionParameter parameter)
        {
            return parameter.BindingSourceMetadata == null;
        }
    }
}