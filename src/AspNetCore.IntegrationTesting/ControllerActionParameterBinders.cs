using System.Collections.Generic;
using System.Linq;
using AspNetCore.IntegrationTesting.Binders;
using AspNetCore.IntegrationTesting.Contracts;

namespace AspNetCore.IntegrationTesting
{
    /// <summary>
    /// A factory class for applying binders to a route
    /// </summary>
    public static class ControllerActionParameterBinders
    {
        /// <summary>
        /// The binders
        /// </summary>
        private static readonly List<IControllerActionRouteBinder> _binders = new List<IControllerActionRouteBinder>(6)
        {
          new FromBodyBinder(),
          new FromFormBinder(),
          new FromHeaderBinder(),
          new FromQueryBinder(),
          new FromRouteBinder(),
          new FromServicesBinder(),
          new DefaultBinder()
        };

        /// <summary>
        /// Adds  additional route binders
        /// </summary>
        /// <param name="binders">The binders.</param>
        public static void AddBinders(params IControllerActionRouteBinder[] binders)
        {
            lock (_binders)
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

        /// <summary>
        /// Binds the specified controller action parameter.
        /// </summary>
        /// <param name="controllerActionParameter">The controller action parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        public static void Bind(IControllerActionParameter controllerActionParameter, IControllerActionRoute controllerActionRoute)
        {
            foreach (var binder in _binders.Where(x => x.CanBind(controllerActionParameter))){
                binder.Bind(controllerActionParameter, controllerActionRoute);
            }
        }
    }
}