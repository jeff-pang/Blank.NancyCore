using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Blank.NancyCore.Core
{
    public class AppMain
    {
        public void Start()
        {
            if (JsonConvert.DefaultSettings == null)
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                };
            }

            string basePath = AppContext.BaseDirectory;

            var webconfig = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("hosting.jcfg", false)
                .Build();
            
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(webconfig)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
