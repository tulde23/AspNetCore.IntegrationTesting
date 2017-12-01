using System.Collections.Generic;
using System.Linq;
using Xunit.AspNetCore.Integration.Contracts;

namespace Xunit.AspNetCore.Integration.Decomposing
{
    /// <summary>
    /// A factory class for applying IControllerActionParameterDecompers to a route
    /// </summary>
    public static class ControllerActionParameterDecomposers
    {
        /// <summary>
        /// The binders
        /// </summary>
        private static readonly List<IControllerActionParameterDecomposer> _binders = new List<IControllerActionParameterDecomposer>(6)
        {
          new FromBodyDecomposer(),
          new FromFormDecomposer(),
          new FromHeaderDecomposer(),
          new FromQueryDecomposer(),
          new FromRouteDecomposer(),
          new FromServicesDecomposer(),
          new DefaultDecomposer()
        };

        /// <summary>
        /// Adds  additional model decomposing binders
        /// </summary>
        /// <param name="binders">The binders.</param>
        public static void AddBinders(params IControllerActionParameterDecomposer[] binders)
        {
            lock (_binders)
            {
                if (binders != null)
                {
                    foreach (var r in binders)
                    {
                        if (!_binders.Any(x => x.GetType().Equals(r.GetType())))
                        {
                            _binders.Add(r);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decomposes the specified controller action parameter.
        /// </summary>
        /// <param name="controllerActionParameter">The controller action parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        public static void Decompose(IControllerActionParameter controllerActionParameter, IControllerActionRoute controllerActionRoute)
        {
            foreach (var binder in _binders.Where(x => x.CanDecompose(controllerActionParameter))){
                binder.Decompose(controllerActionParameter, controllerActionRoute);
            }
        }
    }
}