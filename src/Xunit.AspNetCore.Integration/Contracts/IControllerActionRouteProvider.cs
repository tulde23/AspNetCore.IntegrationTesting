using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit.AspNetCore.Integration.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IControllerActionRouteProvider
    {
        /// <summary>
        /// Creates an IControllerActionRoute from an IControllerAction
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        IControllerActionRoute CreateRoute(IControllerAction action);
    }
}
