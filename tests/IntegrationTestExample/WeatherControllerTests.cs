using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore;
using AwesomeAPI.Controllers;
using AwesomeApiIntegrationTest;
using FluentAssertions;
using IntegrationTestAwesomeApi;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTestExample
{
    public class WeatherControllerTests : AwesomeApiTest
    {
        public WeatherControllerTests(AwesomeApiWebFactory fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper)
        {
        }

     
        [Fact(DisplayName = "Get Produces Two Zip Codes")]
        public async Task TestGet()
        {
            var response = await InvokeAsync<WeatherController, IEnumerable<int>>(controller => controller.Get());
            response.Should().HaveCount(2);
        }

        [Theory(DisplayName = "Get Weather By Postal Code")]
        [MemberData(nameof(ZipCodes))]
        public async Task TestGetByPostalCode(int zipCode, int expectedResult)
        {
            (await InvokeAsyncTask<WeatherController, Task<int>>(controller => controller.GetFromRoute(zipCode)))
          .Should().Be(expectedResult);
        }

        [Theory(DisplayName = "Post Weather By Postal Code")]
        [MemberData(nameof(ZipCodes))]
        public async Task TesPostByPostalCode(int zipCode, int expectedResult)
        {
            (await InvokeAsyncTask<WeatherController, Task<int>>(controller => controller.Post(zipCode)))
          .Should().Be(expectedResult);
        }

        [Theory(DisplayName = "Put Weather By Postal Code")]
        [MemberData(nameof(ZipCodes))]
        public async Task TesPutByPostalCode(int zipCode, int expectedResult)
        {
            (await InvokeAsyncTask<WeatherController, Task<int>>(controller => controller.Put(zipCode)))
          .Should().Be(expectedResult);
        }

        [Theory(DisplayName = "Get Weather By Postal Code With Route Attribute")]
        [MemberData(nameof(ZipCodes))]
        public async Task TestGetByPostalCodeWithAttribute(int zipCode, int expectedResult)
        {
            (await InvokeAsyncTask<WeatherController, Task<int>>(controller => controller.GetFromRouteWithAttribute(zipCode)))
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