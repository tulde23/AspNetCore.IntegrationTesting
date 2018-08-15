using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace AspNetCore.IntegrationTesting
{
    /// <summary>
    /// Provides a base xunit test fixture for integration tests
    /// </summary>
    /// <typeparam name="TStartup">The type of the startup.</typeparam>
    public abstract class AbstractIntegrationTestWebFactory<TStartup> : WebApplicationFactory<TStartup>, ILoggableTestFixture where TStartup : class
    {
        /// <summary>
        /// Gets the relative content root.
        /// </summary>
        /// <returns></returns>
        public abstract string GetRelativeContentRoot();
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractIntegrationTestFixture{TStartup}"/> class.
        /// </summary>
        public AbstractIntegrationTestWebFactory()
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSolutionRelativeContentRoot(GetRelativeContentRoot());
        }
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
               .UseStartup<TStartup>();
        }
    }
}