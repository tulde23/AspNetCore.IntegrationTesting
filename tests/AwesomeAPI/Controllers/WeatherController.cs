using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeAPI.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
   
        /// <summary>
        /// Returns a list of zip codes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<int> Get()
        {
            return new int[] { 19106, 30317 };
        }


        /// <summary>
        /// Get weather for a zip code using a route parameter
        /// </summary>
        /// <param name="postalCode">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{postalCode}")]
        [Produces("application/json")]
        public  async Task<int> GetFromRoute([FromRoute] int postalCode)
        {
            return await GetCurrentTempForZipCode(postalCode);
        }

        // POST api/values
        [HttpPost]
        [Produces("application/json")]
        public async Task<int> Post([FromBody]int postalCode)
        {
            return await GetCurrentTempForZipCode(postalCode);
        }

        // PUT api/values/5
        [HttpPut]
        [Produces("application/json")]
        public async Task<int> Put([FromQuery]int id)
        {
            return await GetCurrentTempForZipCode(id);
        }

        // PUT api/values/5
        [HttpGet]
        [Route("route/{postalCode}")]
        [Produces("application/json")]
        public async Task<int> GetFromRouteWithAttribute([FromRoute]int postalCode)
        {
            return await GetCurrentTempForZipCode(postalCode);
        }
        private Task<int> GetCurrentTempForZipCode(int postalCode)
        {
            switch (postalCode)
            {
                case 19106: return Task.FromResult(55);
                case 30317: return Task.FromResult(65);
                default: return Task.FromResult(1);
            }
        }
    }
}
