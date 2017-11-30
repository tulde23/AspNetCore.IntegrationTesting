using System;
using System.Collections.Generic;
using System.Text;
using Xunit.AspNetCore.Integration.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Xunit.AspNetCore.Integration.Decomposing
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xunit.AspNetCore.Integration.Decomposing.AbstractDecomposer" />
    class FromServicesDecomposer : AbstractDecomposer
    {
        protected override void DecomposeParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {

        }

        public override bool CanDecompose(IControllerActionParameter parameter)
        {
            return IsBindingSourceOfType<FromServicesAttribute>(parameter);
        }
    }
}
