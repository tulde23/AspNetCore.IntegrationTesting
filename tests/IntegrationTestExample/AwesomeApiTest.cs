using AspNetCore.IntegrationTesting;
using AwesomeApiIntegrationTest;
using Xunit.Abstractions;

namespace IntegrationTestAwesomeApi
{
    public class AwesomeApiTest : AbstractClassFixture<AwesomeApiWebFactory, IntegrationTestStartup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwesomeApiTest" /> class.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        public AwesomeApiTest(AwesomeApiWebFactory fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper)
        {
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="AwesomeApiTest" /> class.
        ///// </summary>
        ///// <param name="fixture">The fixture.</param>
        ///// <param name="testOutputHelper">The test output helper.</param>
        //public AwesomeApiTest(AwesomeApiWebFactory fixture, ITestOutputHelper testOutputHelper )
        //    : base(fixture, testOutputHelper)
        //{
        //}
    }
}