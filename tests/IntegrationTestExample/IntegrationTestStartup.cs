using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeAPI;
using Microsoft.AspNetCore.Hosting;

namespace AwesomeApiIntegrationTest
{
    public class IntegrationTestStartup : Startup
    {
        public IntegrationTestStartup(IHostingEnvironment env) : base(env)
        {

        }
    }
}
