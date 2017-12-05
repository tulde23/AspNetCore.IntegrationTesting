using System;
using System.Threading.Tasks;

namespace AspNetCore.IntegrationTesting.Contracts
{
    /// <summary>
    /// Describes an abstraction for invoking an IControllerAction
    /// </summary>
    public interface IControllerActionInvoker : IDisposable
    {
        /// <summary>
        /// Invokes the controller action asynchronously.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="controllerAction">The controller action.</param>
        /// <returns></returns>
        Task<TResponse> InvokeAsync<TResponse>(IControllerAction controllerAction);
    }
}