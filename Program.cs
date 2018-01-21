using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SensorAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())                
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .Build();
    }
}
