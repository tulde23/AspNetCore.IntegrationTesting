# AspNetCore.IntegrationTesting
AspNetCore.IntegrationTesting is a simple library allowing you to run integration tests against your controllers in a strongly typed manner.  No more magic route strings and determining what HttpMethod to invoke. You can invoke your controller with one line of code!

Our goal is to refactor this:
```csharp
  [Theory]
 [MemberData(nameof(CommonData))]
 public async Task GetWeatherForPostalCode(int postalCode, int expectedResult){
  
    var response = await Client.GetAsync($"/api/v1/Weather?postalCode={postalCode}");
    var json = await response.Content.ReadAsStringAsync();
    var data = JsonConvert.DeserializeObject<int>(json);
    data.Should().Equal(expectedResult);
   
  } 
```
Into this:
``` csharp
 [Theory]
 [MemberData(nameof(CommonData))]
 public  async Task GetWeatherForPostalCode(int postalCode, int expectedResult){
    (await Fixture.SingleAsync<WeatherController, int>(controller => controller.Get(postalCode)))
        .Should().Equal(expectedResult);
   
}

```

Let's get started by installing Xunit.AspNetCore.Integration.  This package provides a few abstractions that should make
authoring Xunit integration tests easier.

## XUnit Fixtures
Xunit provides two mechanisms for shared state, the class fixture and the collection fixture.  Read more [here](https://xunit.github.io/docs/shared-context.html) about both.  If we need to share context across tests (classes) we must use a collection fixture.  Collection fixtures are particulary important when running integration tests across multiple classes.  You don't want the overhead of constructing and destroying a TestServer for every class in your test project.


### Building Your First Fixture

Due to the way XUnit implements collection fixtures, there is a slight amount of boring code we must write to get things going.

First we will start with the fixture.
``` csharp
public class AwesomeApiIntegrationTestFixture : AbstractIntegrationTestFixture<Startup>{
	public AwesomeApiIntegrationTestFixture(){
	}
}
```
All we are doing here is stating that we want a fixture that uses the default Startup.cs file residing in your main .net core application.  If your integration test needs to override any behavior in this class, simply extend it and use your new class instead.
``` csharp
public class IntegrationTestStartup : Startup{
}

public class AwesomeApiIntegrationTestFixture : AbstractIntegrationTestFixture<IntegrationTestStartup>{
	public AwesomeApiIntegrationTestFixture(){
	}
}
```
You can also override the default baseAddress used by the HttpClient by using a different base constructor:
``` csharp
public class AwesomeApiIntegrationTestFixture : AbstractIntegrationTestFixture<Startup>{
	public AwesomeApiIntegrationTestFixture() : base( ()=> "http://localhost:8080"){
	}
}
```
Great.  Now our fixture is good to go. Next we need to implement an XUnit collection fixture.
``` csharp
  /// <summary>
    /// This attribute is how you run startup/teardown code in xunit
    /// </summary>
    [CollectionDefinition(nameof(AwesomeApiFixtureCollection))]
    public class AwesomeApiFixtureCollection : ICollectionFixture<AwesomeApiIntegrationTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
```
Lastly, let's create a common base class for all our tests.   This will serve two purposes:
1.  In order to use the collection fixture, we must decorate our class with a CollectionAttribute.  Having a base class eliminates the need to apply for every test class.
2.  The collection fixture gets injected by Xunit at runtime.  Now that we define our attribute on a common base class we can also define a default constructor accepting our fixture.  Now we don't need to think to much when creating our test classes. 

``` csharp
[Collection(nameof(AwesomeApiFixtureCollection))]
public class AwesomeApiTest : AbstractTest<AwesomeApiIntegrationTestFixture>
{
	public AwesomeApiTest(AwesomeApiIntegrationTestFixture fixture) : base(fixture){
	}
}
```

### Our First Integration Test

My tests use [FluentAssertions.](http://fluentassertions.com/examples.html)  It's awesome.

Assume we have the following controller defined:
``` csharp
[Route("api/v1/[controller]")]
public class WeatherController : Controller{
  
 [HttpGet()]
 public Task<int> Get([FromQuery] int postalCode){
  {
     switch(postalCode){
        case 19106 : return Task.FromResult(80);
        case 30317 : return Task.FromResult(70);
        default : return Task.FromResult(1);
     }

   }
}
```

And now for the test.
``` csharp
public class WeatherControllerTests : AwesomeApiTest{
	public WeatherControllerTests(AwesomeApiIntegrationTestFixture fixture) 
           : base(fixture){}
 [Theory]
 [MemberData(nameof(CommonData))]
 public async Task GetWeatherForPostalCode(int postalCode, int expectedResult){
                 (await Fixture.SingleAsync<WeatherController, int>(controller => controller.Get(postalCode)))
                      .Should().Equal(expectedResult);
   
}
   public static TheoryData CommonData
        {
            get
            {
                var data = new TheoryData<int,int>();
                data.Add(19106, 80);
                data.Add(30317, 70);
                data.Add(1, -1);
                return data;
            }
        }
}
```

Not too bad?

### Customization

For most scenarios, the default model binders should be able to map your action parameters back to a URI.  However, in some cases, like custom model binders, you will need to write a bit of code to help out.

First create the route binder:
``` csharp
/// <summary>
    /// Pulls  an id from an inbound model and sets it on the route
    /// </summary>
    /// <seealso cref="AbstractRouteBinder" />
    public class CustomFromModelRouteBinder : AbstractRouteBinder
    {
        /// <summary>
        /// Determines whether this instance can bind the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// <c>true</c> if this instance can decompose the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanBind(IControllerActionParameter parameter)
        {
            var model = parameter.ParameterValue as MyModel;
            return model != null;
        }

        /// <summary>
        /// Binds the parameter to the route.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="controllerActionRoute">The controller action route.</param>
        protected override void BindParameter(IControllerActionParameter parameter, IControllerActionRoute controllerActionRoute)
        {
            var model = parameter.ParameterValue as MyModel;
            controllerActionRoute.SetRouteValue("id", model.PersonId);
        }
    } 
```

Now let's register our binder. You do this in our IntegrationTestFixture constructor.
``` csharp
public class IntegrationTestStartup : Startup{
}

public class AwesomeApiIntegrationTestFixture : AbstractIntegrationTestFixture<IntegrationTestStartup>{
	public AwesomeApiIntegrationTestFixture(){
        ControllerActionParameterBinders.AddBinders(new CustomFromModelBinder());
       
	}
}
```

That's it!  Enjoy.