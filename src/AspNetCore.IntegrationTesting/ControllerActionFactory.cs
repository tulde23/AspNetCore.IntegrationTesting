using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using AspNetCore.IntegrationTesting.Contracts;
using AspNetCore.IntegrationTesting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AspNetCore.IntegrationTesting
{
    /// <summary>
    /// A factory class for creating instances of IControllerAction
    /// </summary>
    internal static class ControllerActionFactory
    {
        /// <summary>
        /// Converts a controller action expression to an instance of IControllerAction.  This instance will contain
        /// all metadata needed to decompose captured parameter values from the caller into an invokable http request.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="controllerActionExpression">The controller action expression.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">You must supply an HttpMethod</exception>
        public static IControllerAction GetAction<TController>(Expression<Func<TController, object>> controllerActionExpression)
        {
            //get the controller type
            var controllerType = typeof(TController);
            //get the controller name.  We assume conventions are used and all controllers end with Controllers suffix.
            var controllerName = controllerType.Name.Replace("Controller", string.Empty);
            //get all controller routeattributes.
            var controllerAttributes = controllerType.GetCustomAttributes<RouteAttribute>(true).Reverse().ToList();
            //down cast to the appropriate expression
            var methodCallExpression = (MethodCallExpression)controllerActionExpression.Body;
            //grab the reflected methodInfo instance
            var methodInfo = methodCallExpression.Method;
            //the return type of the method
            var returnType = methodInfo.ReturnType;
            //grab the methodName
            var methodName = methodInfo.Name;
            //grab a list of method input parameters
            var methodInputParameters = methodInfo.GetParameters();
            //all controller methods must have a derivative of HttpMethodAttribute
            var httpMethodAttribute = methodInfo.GetCustomAttribute<HttpMethodAttribute>(false);
            //get a route attribute applied to controller action
            var routeAttribute = methodInfo.GetCustomAttribute<RouteAttribute>(false);
            if (httpMethodAttribute == null)
            {
                //if no, method, we should throw an exception
                throw new InvalidOperationException("You must supply an HttpMethod");
            }
            var controllerAction = new ControllerAction(controllerName,
                methodName,
                returnType,
                new HttpMethod(httpMethodAttribute.HttpMethods.FirstOrDefault()));

            for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
            {
                var expr = methodCallExpression.Arguments[i];
                var bindingSourceMetadataAttribute = GetBindingSourceAttribute(methodInputParameters[i]);
                controllerAction.ActionParameters.Add(new ControllerActionParameter(methodInputParameters[i].Name,
                    Expression.Lambda(expr).Compile().DynamicInvoke(),
                    bindingSourceMetadataAttribute,
                    controllerAction));
            }
            //if there are any controller scoped route attributes push them into our segment list
            foreach (var a in controllerAttributes)
            {
                controllerAction.RouteSegments.Add(a.Template);
            }

            //process the template on the httpMethod attribute
            if (!string.IsNullOrEmpty(httpMethodAttribute.Template))
            {
                controllerAction.RouteSegments.Add(httpMethodAttribute.Template);
            }
            if (routeAttribute != null && !String.IsNullOrEmpty(routeAttribute.Template))
            {
                controllerAction.RouteSegments.Add(routeAttribute.Template);
            }
            return controllerAction;
        }

        /// <summary>
        /// Gets the annotated binding source attribute.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private static IBindingSourceMetadata GetBindingSourceAttribute(ParameterInfo parameter)
        {
            var bindingSourceMetadata = parameter.GetCustomAttributes().OfType<IBindingSourceMetadata>();
            return bindingSourceMetadata?.FirstOrDefault();
        }
    }
}