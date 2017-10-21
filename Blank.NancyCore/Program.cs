using Blank.NancyCore.Core;
using Serilog;
using System;
using System.IO;

namespace Blank.NancyCore
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = AppContext.BaseDirectory;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Logger(lc =>
                    lc.Filter.ByExcluding(evt => evt.Level == Serilog.Events.LogEventLevel.Debug)
                   .WriteTo.LiterateConsole()
                   .WriteTo.RollingFile(Path.Combine(path, "logs", "{Date}.log")))
                .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(evt => evt.Level == Serilog.Events.LogEventLevel.Debug)
                   .WriteTo.RollingFile(Path.Combine(path, "logs", "{Date}-details.log")))
                .CreateLogger();

            AppMain app = new AppMain();
            app.Start();
        }
    }
}