using System;
using System.Collections.Generic;
using System.Text;
using AwesomeAPI;
using Xunit;
using Xunit.AspNetCore.Integration;
using Xunit.AspNetCore.Integration.Decomposing;

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
    /// <seealso cref="Xunit.AspNetCore.Integration.IntegrationTestFixture{IntegrationTestStartup}" />
    public class AwesomeApiIntegrationTestFixture : IntegrationTestFixture<Startup>
    {
        public AwesomeApiIntegrationTestFixture() : base()
        {
            //this is how we add custom controller action parameter decomposers
            //ControllerActionParameterDecomposers.AddBinders(new MyCustomDecomposer());
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
