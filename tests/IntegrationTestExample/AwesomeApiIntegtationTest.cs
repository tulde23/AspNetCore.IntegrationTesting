using AspNetCore.IntegrationTesting;
using AwesomeAPI;
using Xunit;


namespace IntegrationTestAwesomeApi
{
    /// <summary>
    /// This attribute is how you run startup/teardown code in xunit
    /// </summary>
    [CollectionDefinition(nameof(AwesomeApiIntegrationTestFixtureCollection))]
    public class AwesomeApiIntegrationTestFixtureCollection : ICollectionFixture<AwesomeApiIntegrationTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    /// <summary>
    /// Defines our Integration Test Fixture
    /// </summary>
    /// <seealso cref="Xunit.AspNetCore.Integration.AbstractIntegrationTestFixture{IntegrationTestStartup}" />
    public class AwesomeApiIntegrationTestFixture : AbstractIntegrationTestFixture<Startup>
    {
        /// <summary>
        /// if you application under test does not reside in /src of the solution root, pass a different root
        /// </summary>
        public AwesomeApiIntegrationTestFixture() : base(null, "tests")
        {
            //this is how we add custom controller action parameter decomposers
            //ControllerActionParameterBinders.AddBinders(new MyCustomBinder());
        }
    }

    [Collection(nameof(AwesomeApiIntegrationTestFixtureCollection))]
    public class AwesomeApiIntegrationTest : AbstractTest<AwesomeApiIntegrationTestFixture>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwesomeApiIntegrationTest"/> class.
        /// </summary>
        /// <param name="configuration">The Fixture.</param>
        public AwesomeApiIntegrationTest(AwesomeApiIntegrationTestFixture fixture) : base(fixture)
        {
        }
    }
}