using System.Threading.Tasks;
using AwesomeAPI.Controllers;
using FluentAssertions;
using IntegrationTestAwesomeApi;
using Xunit;

namespace IntegrationTestExample
{
    public class WeatherControllerTests : AwesomeApiIntegrationTest
    {
        public WeatherControllerTests(AwesomeApiIntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact(DisplayName = "Get Produces Two Zip Codes")]
        public async Task TestGet()
        {
            (await Fixture.ManyAsync<WeatherController, int>(controller => controller.Get()))
                .Should().HaveCount(2);
        }

        [Theory(DisplayName = "Get Weather By Postal Code")]
        [MemberData(nameof(ZipCodes))]
        public async Task TestGetByPostalCode(int zipCode, int expectedResult)
        {
            (await Fixture.SingleAsync<WeatherController, int>(controller => controller.GetFromRoute(zipCode)))
          .Should().Be(expectedResult);
        }

        [Theory(DisplayName = "Post Weather By Postal Code")]
        [MemberData(nameof(ZipCodes))]
        public async Task TesPostByPostalCode(int zipCode, int expectedResult)
        {
            (await Fixture.SingleAsync<WeatherController, int>(controller => controller.Post(zipCode)))
          .Should().Be(expectedResult);
        }

        [Theory(DisplayName = "Put Weather By Postal Code")]
        [MemberData(nameof(ZipCodes))]
        public async Task TesPutByPostalCode(int zipCode, int expectedResult)
        {
            (await Fixture.SingleAsync<WeatherController, int>(controller => controller.Put(zipCode)))
          .Should().Be(expectedResult);
        }
        [Theory(DisplayName = "Get Weather By Postal Code With Route Attribute")]
        [MemberData(nameof(ZipCodes))]
        public async Task TestGetByPostalCodeWithAttribute(int zipCode, int expectedResult)
        {

         
            (await Fixture.SingleAsync<WeatherController, int>(controller => controller.GetFromRouteWithAttribute(zipCode)))
          .Should().Be(expectedResult);
        }

        public static TheoryData ZipCodes
        {
            get
            {
                var data = new TheoryData<int, int>();
                data.Add(19106, 55);
                data.Add(30317, 65);
                data.Add(-88, 1);
                return data;
            }
        }
    }
}