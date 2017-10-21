using Blank.NancyCore.Abstract.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blank.AspNetCore.Core.IocRegistry
{
    public class IocConfig
    {
        public string ImplProject { get; set; }

        public static IocConfig GetConfig()
        {
            var config = SystemConfig.GetConfig<IocConfig>("ioc");
            return config;
        }
    }
}
