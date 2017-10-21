using Nancy;
using Nancy.IO;
using Nancy.Extensions;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Serilog;
using System;
using System.Text;
using System.Linq;
using StructureMap;
using Blank.NancyCore.Core.IocRegistry;
using Blank.NancyCore.Abstract;

namespace Blank.NancyCore.Core.NancyCustom
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            var registry = registrations();
            var map = new Container(registry);
            var menuBuilder = map.GetInstance<IMenuBuilder>();
            container.Register<IMenuBuilder>(menuBuilder);
            pipelines.OnError += (ctx, ex) => {

                string path = ctx.Request.Path;
                string clientIp = ctx.Request.UserHostAddress;

                string nginxClientIp = ctx.Request.Headers["X-Real-IP"]?.FirstOrDefault();
                if (!string.IsNullOrEmpty(nginxClientIp))
                {
                    clientIp = nginxClientIp;
                }

                Log.Error("{ip}\t{path}\t{message}", clientIp, path, ex.Message);

                try
                {
                    string body = RequestStream.FromStream(ctx.Request.Body).AsString();
                    var loginfo = new StringBuilder();
                    loginfo.AppendLine(ex.Message);

                    loginfo.AppendFormat("Content-Type: --- {0} ---\n", ctx.Request.Headers.ContentType?.ToString());
                    loginfo.AppendFormat("Request Body: {0}\n\t--- End of Request Body ---\n", body.Replace("\n", "\n\t"));
                    Log.Debug(ex, loginfo.ToString());
                }
                catch (Exception lex)
                {
                    Log.Error("{ip}\t{path}\t{message}", clientIp, path, lex.Message);
                    Log.Debug(lex, lex.Message);
                }

                return null;
            };
        }
        
        private Registry registrations()
        {
            var registry = new Registry();

            registry.IncludeRegistry<MenuBuilderRegistry>();

            return registry;
        }

    }
}