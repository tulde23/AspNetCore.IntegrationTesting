using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AspNetCore.IntegrationTesting;
using AspNetCore.IntegrationTesting.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;

namespace Xunit.AspNetCore.Integration
{
    /// <summary>
    /// Provides a base xunit test fixture for integration tests
    /// </summary>
    /// <typeparam name="TStartup">The type of the startup.</typeparam>
    public abstract class AbstractIntegrationTestFixture<TStartup> where TStartup : class
    {
        /// <summary>
        /// Instance of the TestServer
        /// </summary>
        private readonly TestServer _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractIntegrationTestFixture{TStartup}"/> class.
        /// </summary>
        public AbstractIntegrationTestFixture() : this("http://localhost:5000")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractIntegrationTestFixture{TStartup}" /> class.
        /// </summary>
        /// <param name="baseAddress">Allows you to override the default base address of localhost:5000</param>
        /// <param name="relativeRoot">The relative root.</param>
        /// <example>
        /// public class MyIntegrationTestFixture : IntegrationTestFixture{
        /// public MyIntegrationTestFixture() : base( ()=&gt; "http://localhost:8080"){
        /// }
        /// }
        /// </example>
        protected AbstractIntegrationTestFixture(Func<string> baseAddress, string relativeRoot = "src") : this(baseAddress?.Invoke() ?? "http://localhost:5000", relativeRoot)
        {
        }

        /// <summary>
        /// Private constructor performing common initialization
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="relativeRoot">The relative root from which to start looking for the main application under test</param>
        private AbstractIntegrationTestFixture(string baseAddress, string relativeRoot = "src")
        {
            var relativeTargetProjectParentDir = Path.Combine(relativeRoot);
            var contentRoot = GetProjectPathFromAncestors(relativeTargetProjectParentDir, typeof(TStartup));
            var builder = new WebHostBuilder()
                .CaptureStartupErrors(true)
                .UseContentRoot(contentRoot)
               .UseStartup(typeof(TStartup));

            _server = new TestServer(builder);
            Services = _server.Host.Services;
            ActionInvoker = _server.GetActionInvoker(baseAddress);
        }

        /// <summary>
        /// Get the ActionInvoker instance.  This allows you to make strongly bound http requests to your controllers
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public IControllerActionInvoker ActionInvoker { get; }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            ActionInvoker.Dispose();
            _server.Dispose();
        }

        /// <summary>
        /// Translates a controller action into an http request using the InProc TestServer.  This method assume the response returns an ApiResponse with a collection
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TResponse>> ManyAsync<TController, TResponse>(Expression<Func<TController, object>> expression) where TController : Controller
        {
            return await ActionInvoker.InvokeAsync<IEnumerable<TResponse>>(ControllerActionFactory.GetAction(expression));
        }

        /// <summary>
        /// Translates a controller action into an http request using the InProc TestServer.  This method assume the response returns an ApiResponse with a single item
        /// of items
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public virtual async Task<TResponse> SingleAsync<TController, TResponse>(Expression<Func<TController, object>> expression) where TController : Controller
        {
            return await ActionInvoker.InvokeAsync<TResponse>(ControllerActionFactory.GetAction(expression));
        }

        /// <summary>
        /// Using the supplied Startup class attempts to correctly locate our project root.  We traverse the ancestor tree in case
        /// the integration tests derives from the application's startup class
        /// </summary>
        /// <param name="projectRelativePath">The project relative path.</param>
        /// <param name="startupType">Type of the startup.</param>
        /// <returns></returns>
        private static string GetProjectPathFromAncestors(string projectRelativePath, Type startupType)
        {
            //get all ansectors type from current type
            Type type = startupType;
            DirectoryNotFoundException lastException = null;
            while (type != null)
            {
                try
                {
                    var path = GetProjectPath(projectRelativePath, type.Assembly);
                    return path;
                }
                catch (DirectoryNotFoundException d)
                {
                    lastException = d;
                }
                type = type.BaseType;
            }
            if (lastException != null)
            {
                throw lastException;
            }
            return null;
        }

        /// <summary>
        /// Gets the full path to the target project that we wish to test
        /// </summary>
        /// <param name="projectRelativePath">The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites</param>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>
        /// The full path to the target project.
        /// </returns>
        /// <exception cref="Exception"></exception>
        private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = AppContext.BaseDirectory;// new FileInfo(startupAssembly.Location).Directory.FullName;
            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            }
            while (directoryInfo.Parent != null);

            throw new DirectoryNotFoundException($"Project root could not be located using the application root {applicationBasePath}.");
        }
    }
}