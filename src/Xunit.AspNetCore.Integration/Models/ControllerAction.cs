using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit.AspNetCore.Integration.Contracts;

namespace Xunit.AspNetCore.Integration.Models
{
    internal class ControllerAction : IControllerAction
    {
        public string Controller { get; }
        public string ActionName { get; }
        public Type ReturnType { get; }
        public HttpMethod Method { get; }
        public List<IControllerActionParameter> ActionParameters { get; }
        public List<string> RouteSegments { get; }

        internal ControllerAction(string controller, string action, Type returnType, HttpMethod method)
        {
            Controller = controller;
            ActionName = action;
            ReturnType = returnType;
            ActionParameters = new List<IControllerActionParameter>();
            RouteSegments = new List<string>();
            Method = method;
        }
    }
}
