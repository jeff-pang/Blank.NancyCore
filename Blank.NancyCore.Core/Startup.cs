using Microsoft.AspNetCore.Builder;
using Nancy.Owin;


namespace Blank.NancyCore.Core
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app
            .UseOwin(x => x.UseNancy());
        }
    }
}
