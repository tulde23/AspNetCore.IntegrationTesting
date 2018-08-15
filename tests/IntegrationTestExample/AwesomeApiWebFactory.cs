using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.IntegrationTesting;
using AwesomeAPI;

namespace AwesomeApiIntegrationTest
{
    public class AwesomeApiWebFactory : AbstractIntegrationTestWebFactory<IntegrationTestStartup>
    {
        public override string GetRelativeContentRoot()
        {
            return @"tests\AwesomeAPI";
        }
    }
}
