namespace AspNetCore.IntegrationTesting
{
    /// <summary>
    /// A common base class for all tests that require a fixture
    /// </summary>
    /// <typeparam name="TFixture">A fixture for this test </typeparam>
    public abstract class AbstractTest<TFixture> where TFixture : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTest{TFixture}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected AbstractTest(TFixture configuration)
        {
            Fixture = configuration;
        }

        /// <summary>
        /// Gets or sets the common fixture.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public TFixture Fixture { get; set; }
    }
}