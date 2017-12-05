using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.IntegrationTesting;
using AspNetCore.IntegrationTesting.Contracts;

namespace Microsoft.AspNetCore.TestHost
{
    /// <summary>
    /// Useful TestServer extension methods
    /// </summary>
    public static class TestServerExtensions
    {
        /// <summary>
        /// Gets an instance of the IControllerActionInvoker
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="baseAddress">The base address.</param>
        /// <returns></returns>
        public static IControllerActionInvoker GetActionInvoker(this TestServer server, string baseAddress)
        {
            var client = server.CreateClient();
            client.BaseAddress = new Uri(baseAddress);
            return new ControllerActionHttpClientDecorator(client);
        }
    }
}
